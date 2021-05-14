using System;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;


public class SaveManager: Singleton<SaveManager>
{
    [SerializeField] string cloudSaveName;
    [SerializeField] DataSource dataSource;
    [SerializeField] ConflictResolutionStrategy conflictResolutionStrategy;

    protected string SaveKey { get { return typeof(SavedValues).ToString(); } }
    public Action OnLoaded { get; set; }
    public Action OnSaved { get; set; }
    public SavedValues SavedValues { get; set; }

    private void OpenCloud(Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        var platform = (PlayGamesPlatform)Social.Active;
        platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictResolutionStrategy, callback);

    }
    public void SaveCloud(Action<SavedGameRequestStatus> callback = null, bool deleteLocalSave = false)
    {
        if (SocialManager.Instance.isConnectedToGooglePlayServices)
        {
            OpenCloud(OnSaveResponse);
        }
        else
        {
            SaveLocal();
            OnSaved?.Invoke();
            callback?.Invoke(SavedGameRequestStatus.AuthenticationError);
        }

        void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                byte[] data = SaveSystemBinary.SerializeState(SavedValues);
                SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                    .WithUpdatedDescription("Last save: " + DateTime.Now.ToString())
                    .Build();
                var platform = (PlayGamesPlatform)Social.Active;
                platform.SavedGame.CommitUpdate(metadata, update, data, SaveCallback);
            }
            else
            {
                SaveLocal();
                OnSaved?.Invoke();
                callback?.Invoke(status);
            }
        }
        void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                OnSaved?.Invoke();
                callback?.Invoke(status);
                if (deleteLocalSave) DeleteLocalSave();
            }
            else
            {
                SaveLocal();
                OnSaved?.Invoke();
                callback?.Invoke(status);
            }
        }
    }
    public void Load(Action<SavedGameRequestStatus> callback = null)
    {
        if (SocialManager.Instance.isConnectedToGooglePlayServices)
        {
            OpenCloud(OnLoadResponse);
        }
        else
        {
            SavedValues = LoadLocal();
            callback?.Invoke(SavedGameRequestStatus.AuthenticationError);
            OnLoaded?.Invoke();
        }

        void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                var platform = (PlayGamesPlatform)Social.Active;
                platform.SavedGame.ReadBinaryData(metadata, LoadCallback);
            }
            else
            {
                SavedValues = LoadLocal();
                callback?.Invoke(status);
                OnLoaded?.Invoke();
            }
        }
        void LoadCallback(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                SavedValues = SaveSystemBinary.DeserializeState<SavedValues>(data);
                OnLoaded?.Invoke();
            }
            else
            {
                SavedValues = LoadLocal();
                OnLoaded?.Invoke();
            }
            callback?.Invoke(status);
        }
    }

    public void SaveLocal()
    {
        SaveSystemBinary.Save(SavedValues, SaveKey);
    }
    public bool LocalSaveExist()
    {
        return SaveSystemBinary.SaveExists(SaveKey);
    }
    SavedValues LoadLocal()
    {
        if (LocalSaveExist())
        {
            return SaveSystemBinary.Load<SavedValues>(SaveKey);
        }
        else
            return new SavedValues();
    }
    public void DeleteLocalSave()
    {
        SaveSystemBinary.DeleteSave(SaveKey);
        SavedValues = LoadLocal();
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        SaveLocal();
    }
    private void OnApplicationQuit()
    {
        SaveLocal();
    }
}
