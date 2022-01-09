using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPCore : MonoBehaviour, IStoreListener 
{
    [SerializeField] private GameObject panelAds;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static string noads = "noads"; 
    public static string gems666 = "gems666"; 
    public static string gems1000 = "gems1000"; 

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) 
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noads, ProductType.NonConsumable);
        builder.AddProduct(gems666, ProductType.Consumable);
        builder.AddProduct(gems1000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void Buy_noads()
    {
        BuyProductID(noads);
    }


    public void Buy_gems1000()
    {
        BuyProductID(gems1000);
    }
    
    public void Buy_gems666()
    {
        BuyProductID(gems666);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
    {
        if (String.Equals(args.purchasedProduct.definition.id, noads, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                panelAds.SetActive(false);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, gems666, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            GameManager.instance.currentGems += 666;
            ShopManager.instance.UpdateCurrentGems(GameManager.instance.currentGems);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, gems1000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            GameManager.instance.currentGems += 1000;
            ShopManager.instance.UpdateCurrentGems(GameManager.instance.currentGems);
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases() 
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }



}