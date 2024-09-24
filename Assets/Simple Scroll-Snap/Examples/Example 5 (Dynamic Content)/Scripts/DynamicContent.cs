// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class DynamicContent : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private Sprite[] arrowSpriteArray;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private Character character;

        private Queue<GameObject> arrowPrefabList = new Queue<GameObject>();
        private Vector2 arrowDirection;
        #endregion

        #region Methods
        private void Awake()
        {
            gameInput.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;
            character.GetCharacterMovement().OnCharacterMoved += CharacterMovement_OnCharacterMoved;
        }

        private void CharacterMovement_OnCharacterMoved(object sender, EventArgs e)
        {
            RemoveFromBack();
        }

        private void GameInput_OnMovementKeyPressed(object sender, GameInput.OnMovementKeyPressedEventArgs e)
        {
            if (GameManager.Instance.GetSelectedCharacter() == character)
            {
                arrowDirection = e.direction;
                Add(0);
            }    
        }

        public void Add(int index)
        {
            // Panel
            if (arrowDirection == Vector2.up)
            {
                arrowPrefab.GetComponent<Image>().sprite = arrowSpriteArray[0];
            }
            else if (arrowDirection == Vector2.left)
            {
                arrowPrefab.GetComponent<Image>().sprite = arrowSpriteArray[1];
            }
            else if (arrowDirection == Vector2.down)
            {
                arrowPrefab.GetComponent<Image>().sprite = arrowSpriteArray[2];
            }
            else if (arrowDirection == Vector2.right)
            {
                arrowPrefab.GetComponent<Image>().sprite = arrowSpriteArray[3];
            }
            arrowPrefabList.Enqueue(scrollSnap.Add(arrowPrefab, index));
        }
        public void AddToFront()
        {
            Add(0);
        }
        public void AddToBack()
        {
            Add(scrollSnap.NumberOfPanels);
        }

        public void Remove(int index)
        {
            if (scrollSnap.NumberOfPanels > 0)
            {
                // Pagination
                //DestroyImmediate(scrollSnap.Pagination.transform.GetChild(scrollSnap.NumberOfPanels - 1).gameObject);

                // Panel
                scrollSnap.Remove(index);
            }
        }
        public void RemoveFromFront()
        {
            Remove(0);
        }
        public void RemoveFromBack()
        {
            if (scrollSnap.NumberOfPanels > 0)
            {
                Remove(scrollSnap.NumberOfPanels - 1);
            }
            else
            {
                Remove(0);
            }
        }
        #endregion
    }
}