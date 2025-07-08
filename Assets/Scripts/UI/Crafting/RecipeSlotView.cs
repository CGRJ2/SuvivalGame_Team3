using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotView : MonoBehaviour
{
     public Item_Recipe recipeItem;
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite_BlockedSlot;
    [SerializeField] private TMP_Text text;
    

    public void SetRecipeSprite()
    {
        image.sprite = recipeItem.RecipeData.resultItem.imageSprite;
        text.text = recipeItem.RecipeData.resultItem.itemName;
    }

    public void SetBlockedSprite()
    {
        image.sprite = sprite_BlockedSlot;
        text.text = "잠긴 레시피";
    }

}
