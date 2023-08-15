﻿using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rimaethon.TileMapping.com.unity._2d.tilemap.extras.Runtime.Tiles.AnimatedTile
{
    /// <summary>
    /// Animated Tiles are tiles which run through and display a list of sprites in sequence.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Animated Tile", menuName = "2D Extras/Tiles/Animated Tile", order = 359)]
    public class AnimatedTile : TileBase
    {
        /// <summary>
        /// The List of Sprites set for the Animated Tile.
        /// This will be played in sequence.
        /// </summary>
        public Sprite[] m_AnimatedSprites;
        /// <summary>
        /// The minimum possible speed at which the Animation of the Tile will be played.
        /// A speed value will be randomly chosen between the minimum and maximum speed.
        /// </summary>
        public float m_MinSpeed = 1f;
        /// <summary>
        /// The maximum possible speed at which the Animation of the Tile will be played.
        /// A speed value will be randomly chosen between the minimum and maximum speed.
        /// </summary>
        public float m_MaxSpeed = 1f;
        /// <summary>
        /// The starting time of this Animated Tile.
        /// This allows you to start the Animation from time in the list of Animated Sprites depending on the 
        /// Tilemap's Animation Frame Rate.
        /// </summary>
        public float m_AnimationStartTime;
        /// <summary>
        /// The starting frame of this Animated Tile.
        /// This allows you to start the Animation from a particular Sprite in the list of Animated Sprites.
        /// If this is set, this overrides m_AnimationStartTime. 
        /// </summary>
        public int m_AnimationStartFrame = 0;
        /// <summary>
        /// The Collider Shape generated by the Tile.
        /// </summary>
        public Tile.ColliderType m_TileColliderType;

        /// <summary>
        /// Retrieves any tile rendering data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0)
            {
                tileData.sprite = m_AnimatedSprites[m_AnimatedSprites.Length - 1];
                tileData.colliderType = m_TileColliderType;
            }
        }

        /// <summary>
        /// Retrieves any tile animation data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileAnimationData">Data to run an animation on the tile.</param>
        /// <returns>Whether the call was successful.</returns>
        public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
        {
            if (m_AnimatedSprites.Length > 0)
            {
                tileAnimationData.animatedSprites = m_AnimatedSprites;
                tileAnimationData.animationSpeed = Random.Range(m_MinSpeed, m_MaxSpeed);
                tileAnimationData.animationStartTime = m_AnimationStartTime;
                if (0 < m_AnimationStartFrame && m_AnimationStartFrame <= m_AnimatedSprites.Length)
                {
                    var tilemapComponent = tileMap.GetComponent<Tilemap>();
                    if (tilemapComponent != null && tilemapComponent.animationFrameRate > 0)
                        tileAnimationData.animationStartTime = (m_AnimationStartFrame - 1) / tilemapComponent.animationFrameRate;
                }
                return true;
            }
            return false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AnimatedTile))]
    public class AnimatedTileEditor : Editor
    {
        private AnimatedTile tile { get { return (target as AnimatedTile); } }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            int count = EditorGUILayout.DelayedIntField("Number of Animated Sprites", tile.m_AnimatedSprites != null ? tile.m_AnimatedSprites.Length : 0);
            if (count < 0)
                count = 0;
                
            if (tile.m_AnimatedSprites == null || tile.m_AnimatedSprites.Length != count)
            {
                Array.Resize<Sprite>(ref tile.m_AnimatedSprites, count);
            }

            if (count == 0)
                return;

            EditorGUILayout.LabelField("Place sprites shown based on the order of animation.");
            EditorGUILayout.Space();

            for (int i = 0; i < count; i++)
            {
                tile.m_AnimatedSprites[i] = (Sprite) EditorGUILayout.ObjectField("Sprite " + (i+1), tile.m_AnimatedSprites[i], typeof(Sprite), false, null);
            }
            
            float minSpeed = EditorGUILayout.FloatField("Minimum Speed", tile.m_MinSpeed);
            float maxSpeed = EditorGUILayout.FloatField("Maximum Speed", tile.m_MaxSpeed);
            if (minSpeed < 0.0f)
                minSpeed = 0.0f;
                
            if (maxSpeed < 0.0f)
                maxSpeed = 0.0f;
                
            if (maxSpeed < minSpeed)
                maxSpeed = minSpeed;
            
            tile.m_MinSpeed = minSpeed;
            tile.m_MaxSpeed = maxSpeed;

            using (new EditorGUI.DisabledScope(0 < tile.m_AnimationStartFrame && tile.m_AnimationStartFrame <= tile.m_AnimatedSprites.Length))
            {
                tile.m_AnimationStartTime = EditorGUILayout.FloatField("Start Time", tile.m_AnimationStartTime);    
            }

            tile.m_AnimationStartFrame = EditorGUILayout.IntField("Start Frame", tile.m_AnimationStartFrame);

            tile.m_TileColliderType=(Tile.ColliderType) EditorGUILayout.EnumPopup("Collider Type", tile.m_TileColliderType);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);
        }
    }
#endif
}
