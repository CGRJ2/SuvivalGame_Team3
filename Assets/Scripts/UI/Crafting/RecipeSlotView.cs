using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotView : MonoBehaviour
{
     public Item_Recipe recipeItem;
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite_BlockedSlot;
    

    public void SetRecipeSprite()
    {
        image.sprite = recipeItem.imageSprite;
    }

    public void SetBlockedSprite()
    {
        image.sprite = sprite_BlockedSlot;
    }

}
