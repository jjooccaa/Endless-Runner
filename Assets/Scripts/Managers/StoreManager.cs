using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;


public class StoreManager : Singleton<StoreManager>
{
    [Header("Items")]
    [SerializeField] Item[] store;
    static public List<Item> inventory = new List<Item>();
    List<GameObject> inventoryObjects = new List<GameObject>();

    [Header("UI")]
    [SerializeField] GameObject storeContentArea;
    [SerializeField] GameObject inventoryContentArea;
    [SerializeField] GameObject itemButton;

    const string CATALOG_ITEMS_NAME = "Version 1";

    private void OnEnable()
    {
        EventManager.Instance.onLoginSuccess += GetCatalog; // FIXME: Not working for some reason. 
        EventManager.Instance.onLoginSuccess += DisplayStoreItems;
    }

    public void GetCatalog()
    {
        GetCatalogItemsRequest request = new GetCatalogItemsRequest
        {
            CatalogVersion = CATALOG_ITEMS_NAME
        };
        PlayFabClientAPI.GetCatalogItems(request, OnGetCatalogSuccess, PlayFabManager.Instance.OnError);
    }

    void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        List<CatalogItem> catalogItems = result.Catalog;
        foreach (CatalogItem catalogItem in catalogItems)
        {
            uint cost = catalogItem.VirtualCurrencyPrices[PlayFabManager.GEMS_CODE];
            string description = catalogItem.Description;

            foreach (Item storeItem in store)
            {
                if (storeItem.iD == catalogItem.ItemId)
                {
                    storeItem.AssignItemInfo((int)cost, description);
                }
            }
        }
        GetInventory();
    }

    void MakePurchase(Item item)
    {
        PurchaseItemRequest request = new PurchaseItemRequest
        {
            CatalogVersion = CATALOG_ITEMS_NAME,
            ItemId = item.iD,
            VirtualCurrency = PlayFabManager.GEMS_CODE,
            Price = item.cost
        };
        PlayFabClientAPI.PurchaseItem(request, OnMakePurchaseSuccess, PlayFabManager.Instance.OnError);
    }

    void OnMakePurchaseSuccess(PurchaseItemResult result)
    {
        GetInventory();
    }

    void GetInventory()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, OnGetInventorySuccess, PlayFabManager.Instance.OnError);
    }

    void OnGetInventorySuccess(GetUserInventoryResult result)
    {
        ClearInventory();
        List<ItemInstance> userInventory = result.Inventory;

        foreach (ItemInstance inventoryItem in userInventory)
        {
            foreach(Item item in store)
            {
                if(inventoryItem.ItemId == item.iD)
                {
                    DisplayItemInInventory(item);
                }
            }
        }
    }

    void ClearInventory()
    {
        inventory.Clear();
        foreach(GameObject gameObject in inventoryObjects)
        {
            Destroy(gameObject);
        }
        inventoryObjects.Clear();
    }

    void DisplayItemInInventory(Item item)
    {
        inventory.Add(item);

        GameObject gameObject = Instantiate(itemButton, inventoryContentArea.transform.position, Quaternion.identity);
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = item.name;
        gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = item.description;
        gameObject.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        gameObject.GetComponent<Image>().preserveAspect = true;
        gameObject.transform.SetParent(inventoryContentArea.transform);
        inventoryObjects.Add(gameObject);
    }

    void DisplayStoreItems()
    {
        foreach (Item item in store)
        {
            GameObject gameObject = Instantiate(itemButton, storeContentArea.transform.position, Quaternion.identity);
            gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = item.name;
            gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format("{0} Gems", item.cost);
            gameObject.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<Image>().preserveAspect = true;
            gameObject.transform.SetParent(storeContentArea.transform);
            gameObject.GetComponent<Button>().onClick.AddListener(delegate { MakePurchase(item); });
        }
    }
}
