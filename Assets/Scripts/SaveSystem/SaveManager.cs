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

    private void Awake()
    {
        SocialManager.Instance.OnAuthenticated += Load;
    }
    private void OpenCloud(Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        var platform = (PlayGamesPlatform)Social.Active;
        platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictResolutionStrategy, callback);

    }
    public void SaveCloud()
    {
        if (SocialManager.Instance.isConnectedToGooglePlayServices)
        {
            OpenCloud(OnSaveResponse);
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("SaveCloud");
        }
#endif
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
            }
        }
        void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                OnSaved?.Invoke();
            }
            else
            {
                SaveLocal();
                OnSaved?.Invoke();
            }
        }
    }
    void Load()
    {
        if (SocialManager.Instance.isConnectedToGooglePlayServices)
        {
            OpenCloud(OnLoadResponse);
        }
        else
        {
            SavedValues = LoadLocal();
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
        }
    }

    public void SaveLocal()
    {
        SaveSystemBinary.Save(SavedValues, SaveKey);
    }
    public bool SaveExist()
    {
        return SaveSystemBinary.SaveExists(SaveKey);
    }
    SavedValues LoadLocal()
    {
        if (SaveExist())
        {
            return SaveSystemBinary.Load<SavedValues>(SaveKey);
        }
        else
            return new SavedValues();
    }
    public void DeleteLocalSave()
    {
        SaveSystemBinary.DeleteSave(SaveKey);
        SavedValues = null;
    }

}
