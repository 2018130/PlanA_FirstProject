using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.IO;
using System.Text;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;


public class GPGSBinder : MonoBehaviour
{

    private static string path = "playerData01.json";
    private static string collectionPath = "collectionData01.json";
    private static string fishbowlPath = "fishbowlData01.json";

    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("LogIn successed");
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication.
            Debug.Log("LogIn failed");
        }
    }

    public static bool CheckLogin()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    public static void SaveToCloud()
    {
        if (!CheckLogin()) return;

        OpenSavedGame(true);
    }
    public static void LoadFromCloud()
    {
        if (!CheckLogin()) return;

        OpenSavedGame(false);
    }

    static void OpenSavedGame(bool bSave)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (bSave)
        {
            savedGameClient.OpenWithAutomaticConflictResolution(path, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedGameOpenedToSavePlayerData);
            savedGameClient.OpenWithAutomaticConflictResolution(collectionPath, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedGameOpenedToSaveCollectionData);
            savedGameClient.OpenWithAutomaticConflictResolution(fishbowlPath, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedGameOpenedToSaveFishbowlData);
        }
        else
        {
            savedGameClient.OpenWithAutomaticConflictResolution(path, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedPlayerDataOpenForRead);
            savedGameClient.OpenWithAutomaticConflictResolution(collectionPath, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedCollectionDataOpenForRead);
            savedGameClient.OpenWithAutomaticConflictResolution(fishbowlPath, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnSavedFishbowlDataOpenForRead);
        }
    }

    static void OnSavedGameOpenedToSavePlayerData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //SaveGame
        Debug.Log("Save player data in cloud");
        PlayerControllerSaveData playercontrollerSaveData = new PlayerControllerSaveData();
        playercontrollerSaveData.Health = PlayerController.SPlayerController.Health;
        playercontrollerSaveData.Coin = PlayerController.SPlayerController.Coin;
        playercontrollerSaveData.FishingLineLenth = PlayerController.SPlayerController.FishingLineLenth;

        string json = JsonUtility.ToJson(playercontrollerSaveData);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        SaveGame(game, bytes);
    }
    static void OnSavedGameOpenedToSaveCollectionData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //SaveGame
        Debug.Log("Save collection data in cloud");
        CollectionSaveData collectionSaveData = new CollectionSaveData();
        for (int i = 0; i < Collection.Instance.fishDatas.Length; i++)
        {
            if (Collection.Instance.isCollected[i])
            {
                collectionSaveData.collectedFishIdx.Add(i);
            }
        }

        string json = JsonUtility.ToJson(collectionSaveData, true);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        SaveGame(game, bytes);
    }
    static void OnSavedGameOpenedToSaveFishbowlData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //SaveGame
        Debug.Log("Save fishbowl data in cloud");
        FishbowlSaveData fishbowlSaveData = new FishbowlSaveData();

        for (int i = 0; i < Fishbowl.Instance.itemSize; i++)
        {
            Debug.Log(Fishbowl.Instance.boxes[i].name);
            Item item = Fishbowl.Instance.boxes[i].GetComponent<Item>();
            fishbowlSaveData.ownItemCount.Add(item.itemCount);
            fishbowlSaveData.ownItemId.Add(item.itemId);
        }

        string json = JsonUtility.ToJson(fishbowlSaveData, true);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        SaveGame(game, bytes);

    }
    static void SaveGame(ISavedGameMetadata game, byte[] savedData)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder.WithUpdatedDescription("Save Game at " + DateTime.Now);

        SavedGameMetadataUpdate updateMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updateMetadata, savedData, (status, game) =>
        {
            if (status != SavedGameRequestStatus.Success)
            {
                Debug.Log("Fail to save data");
            }
            else
            {
                Debug.Log("Success to save data");
            }
        });
    }

    static void OnSavedPlayerDataOpenForRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //ReadFile
        LoadPlayerData(game);
    }

    static void LoadPlayerData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, (status, data) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Succeed to read player data");

                string LoadData = Encoding.UTF8.GetString(data);
                PlayerController.SPlayerController.LoadCloudDataIntoGameData(LoadData);
            }
            else
            {
                Debug.Log("Fail to LoadData");
            }
        });
    }
    static void OnSavedCollectionDataOpenForRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //ReadFile
        LoadCollectionData(game);
    }

    static void LoadCollectionData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, (status, data) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Succeed to read collection data");
                string LoadData = Encoding.UTF8.GetString(data);
                Collection.Instance.LoadCloudDataIntoGameData(LoadData);
            }
            else
            {
                Debug.Log("Fail to LoadData");
            }
        });
    }

    static void OnSavedFishbowlDataOpenForRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            return;
        }

        //ReadFile
        LoadFishbowlData(game);
    }

    static void LoadFishbowlData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, (status, data) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("Succeed to read fishbowl data");
                string LoadData = Encoding.UTF8.GetString(data);
                Fishbowl.Instance.LoadCloudDataIntoGameData(LoadData);
            }
            else
            {
                Debug.Log("Fail to LoadData");
            }
        });
    }
}