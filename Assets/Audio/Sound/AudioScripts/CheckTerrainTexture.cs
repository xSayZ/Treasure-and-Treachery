using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Audio
{
    public class CheckTerrainTexture : MonoBehaviour
    {
        public PlayerAudio playerAudio;
        private Transform playerTransform;
        private Terrain terrainObject;
        public int posX;
        public int posZ;
        public float[] textureValues;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
            playerTransform = gameObject.transform;
            UpdateTerrainReference();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the event to prevent memory leaks
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            UpdateTerrainReference();
        }

        private void UpdateTerrainReference()
        {
            terrainObject = Terrain.activeTerrain;
            if (terrainObject == null)
            {
                Debug.LogWarning("No active terrain found in the scene.");
            }
        }

        public void PlayFootStep()
        {
            if (terrainObject == null)
            {
                Debug.LogWarning("No active terrain found. Footstep detection aborted.");
                return;
            }

            GetTerrainTexture();
            if (textureValues[0] > 0.5f)
            {
                var textureValue = 0;
                playerAudio.PlayerFootstepPlay(textureValue, gameObject);
            }
            if (textureValues[1] > 0.5f)
            {
                var textureValue = 1;
                playerAudio.PlayerFootstepPlay(textureValue, gameObject);
            }
        }

        public void GetTerrainTexture()
        {
            if (terrainObject == null)
            {
                Debug.LogWarning("No active terrain found. Texture detection aborted.");
                return;
            }

            UpdatePosition();
            CheckTexture();
        }

        void UpdatePosition()
        {
            Vector3 terrainPosition = playerTransform.position - terrainObject.transform.position;
            Vector3 mapPosition = new Vector3(terrainPosition.x / terrainObject.terrainData.size.x, 0,
                terrainPosition.z / terrainObject.terrainData.size.z);
            float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
            float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight;
            posX = (int)xCoord;
            posZ = (int)zCoord;
        }

        void CheckTexture()
        {
            float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX, posZ, 1, 1);
            textureValues[0] = splatMap[0, 0, 0];
            textureValues[1] = splatMap[0, 0, 1];
        }
    }
}
