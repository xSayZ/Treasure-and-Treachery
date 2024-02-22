// /*------------------------------
// --------------------------------
// Creation Date: 2024/02/12
// Author: herman
// Description: Operation_Donken
// --------------------------------
// ------------------------------*/

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Game {
    namespace Audio {
        public class CheckTerrainTexture : MonoBehaviour
        {
            public PlayerAudio playerAudio;
            public Transform playerTransform;
            private Terrain terrainObject;

            public int posX;
            public int posZ;
            public float[] textureValues;

        #region Unity Functions
        void Start()
        {
            UpdateTerrainReference();
            playerTransform = gameObject.transform;
        }

        // void Update()
        // {
        //     GetTerrainTexture();
        // }

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
        
#endregion

#region Public Functions

public void PlayFootStep()
{
    GetTerrainTexture();

    try
    {
        playerAudio.PlayerFootstepPlay(textureValues[0], textureValues[1], gameObject);
    }
    catch (Exception e)
    {
        Debug.LogError("[{Projectile}]: Error Exception " + e);
    }
}

public void GetTerrainTexture()
{
    // Null check
    if (!terrainObject)
    {
        return;
    }
    
    UpdatePosition();
    CheckTexture();
}

#endregion

#region Private Functions

private void UpdateTerrainReference()
{
    terrainObject = Terrain.activeTerrain;
}

#endregion
        }
    }
}