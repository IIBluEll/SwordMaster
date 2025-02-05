using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<SwordSO> inventory = new List<SwordSO>();
    [SerializeField] private int inventorySize = 5;

    [SerializeField] private Transform inventoryParent; // 인벤토리 부모 오브젝트
    //[SerializeField] private TMP_Text swordInfoText;

    private List<Image> inventorySlots = new List<Image>(); // 슬롯을 Image로 변경

    private void Start()
    {
        // 인벤토리 부모 오브젝트에서 슬롯 이미지를 자동으로 가져옴
        foreach (Transform child in inventoryParent)
        {
            Image slotImage = child.Find("Slot").GetComponent<Image>();
            if (slotImage != null)
            {
                inventorySlots.Add(slotImage);
            }
        }

        UpdateInventoryUI();
    }

    public void AddToInventory(SwordSO sword)
    {
        if (inventory.Count >= inventorySize)
        {
            Debug.Log("인벤토리가 가득 찼습니다. 더 이상 검을 추가할 수 없습니다.");
            return;
        }

        inventory.Add(sword);
        Debug.Log($"검 {sword.swordData.swordName_KR}을(를) 인벤토리에 추가했습니다.");

        UpdateInventoryUI();
    }
    
    public void RemoveFromInventory(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            Debug.Log($"검 {inventory[index].swordData.swordName_KR}을(를) 삭제했습니다.");
            inventory.RemoveAt(index);
            UpdateInventoryUI();
        }
    }
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Image slotImage = inventorySlots[i];

            if (i < inventory.Count)
            {
                // 인벤토리에 검이 있는 경우 슬롯 이미지 활성화
                slotImage.sprite = inventory[i].swordData.swordSprite;
                slotImage.enabled = true;
            }
            else
            {
                // 빈 슬롯은 이미지 숨기기
                slotImage.enabled = false;
            }
        }

        // 🔹 인벤토리가 비어 있으면 검 정보 초기화
        if (inventory.Count == 0)
        {
            //swordInfoText.text = "인벤토리가 비어 있습니다.";
        }
    }

    private void ShowSwordInfo(int index)
    {
        if (index < inventory.Count)
        {
            SwordData sword = inventory[index].swordData;
            //swordInfoText.text = $"{sword.swordName_EN}\n{sword.swordLevel} LV\n{(sword.upgradeRate)}%";
        }
    }
}
