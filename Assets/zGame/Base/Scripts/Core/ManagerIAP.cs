// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.Purchasing;
// using UnityEngine.Purchasing.Security;
// using MaskForce;
// using Firebase.Analytics;

// // Created by nt.Dev93
// namespace ntDev
// {
//     [System.Serializable]
//     public class IAPVerifyResult
//     {
//         public string sku = null;
//         public int error = -1;
//         public string errorMessage;
//     }

//     public class ManagerIAP : IStoreListener
//     {
//         static IStoreController m_StoreController;
//         static IExtensionProvider m_StoreExtensionProvider;

//         static ManagerIAP Instance;
//         public void Init()
//         {
//             if (Instance != null) return;
//             Instance = this;
//             InitializePurchasing();
//         }

//         void InitializePurchasing()
//         {
//             if (m_StoreController != null && m_StoreExtensionProvider != null) return;
//             var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//             foreach (IAPData data in IAPData.ListData)
//                 builder.AddProduct(data.Package, data.Sub ? ProductType.Subscription : ProductType.Consumable);

//             UnityPurchasing.Initialize(this, builder);
//         }

//         public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//         {
//             Ez.Log("OnInitialized: PASS");
//             m_StoreController = controller;
//             m_StoreExtensionProvider = extensions;

//             RestoreSub();
//         }

//         public static void RestoreTransactions(Action<bool> action)
//         {
//             Ez.Log("Restore Transaction");
// #if UNITY_ANDROID
//             m_StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>().RestoreTransactions((b) =>
//             {
//                 CoreGame.RunOnMainThread(() => action?.Invoke(b));
//             });
// #elif UNITY_IOS
//             m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((b) =>
//             {
//                 CoreGame.RunOnMainThread(() => action?.Invoke(b));
//             });
// #endif
//         }

//         public static void RestoreSub()
//         {
//             if (m_StoreController == null || m_StoreExtensionProvider == null) return;
//             foreach (IAPData data in IAPData.ListData)
//                 if (data.Sub) Instance.CheckSub(data.Package);
//         }

//         void CheckSub(string package)
//         {
//             Ez.Log("Check Sub: " + package);

//             // string intro_json = null;
//             // #if UNITY_ANDROID
//             // #else
//             //             string storeSpecificId = m_StoreController.products.WithID(package).definition.storeSpecificId;
//             //             Dictionary<string, string> dict = m_StoreExtensionProvider.GetExtension<IAppleExtensions>().GetIntroductoryPriceDictionary();
//             //             Ez.Log("StoreSpecificId: " + m_StoreController.products.WithID(package).definition.storeSpecificId);
//             //             if (dict == null) Ez.Log("IntroductoryPriceDictionary NULL");
//             //             else if (!dict.ContainsKey(storeSpecificId)) Ez.Log("IntroductoryPriceDictionary NoKEY");
//             //             else Ez.Log("Intro_json: " + dict[storeSpecificId]);
//             //             intro_json = (dict == null || !dict.ContainsKey(storeSpecificId)) ? null : dict[storeSpecificId];
//             // #endif
//             try
//             {
//                 SubscriptionManager subscriptionManager = new SubscriptionManager(m_StoreController.products.WithID(package), null);
//                 if (subscriptionManager != null)
//                 {
//                     var subScribleInfo = subscriptionManager.getSubscriptionInfo();
//                     if (subScribleInfo != null)
//                     {
//                         var result = subScribleInfo.isSubscribed();

//                         var isSubcribled = subScribleInfo.isSubscribed() == UnityEngine.Purchasing.Result.True;
//                         var isExpired = subScribleInfo.isExpired() == UnityEngine.Purchasing.Result.True;
//                         var isCancelled = subScribleInfo.isCancelled() == UnityEngine.Purchasing.Result.True;
//                         var thisGameSubcribled = isSubcribled && !isExpired && !isCancelled;

//                         SaveGame.SubVip = thisGameSubcribled;
//                         Ez.Log("VIP isSubcribled " + isSubcribled);
//                         Ez.Log("VIP isExpired " + isExpired);
//                         Ez.Log("VIP isCancelled " + isCancelled);
//                         Ez.Log("VIP " + SaveGame.SubVip);
//                     }
//                 }
//                 else Ez.Log("SubscriptionManager null");
//             }
//             catch (Exception e) { Ez.Log(e.ToString()); }
//         }

//         public void OnInitializeFailed(InitializationFailureReason error)
//         {
//             Ez.Log("OnInitializeFailed InitializationFailureReason:" + error);
//         }

//         public void RestorePurchases()
//         {
//             if (m_StoreController != null && m_StoreExtensionProvider != null)
//             {
//                 if (Application.platform == RuntimePlatform.IPhonePlayer ||
//                     Application.platform == RuntimePlatform.OSXPlayer)
//                 {
//                     var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//                     apple.RestoreTransactions((result) => { });
//                 }
//             }
//         }

//         public static string GetLocalizedPriceString(string pPackageId)
//         {
//             try
//             {
//                 var product = m_StoreController.products.WithID(pPackageId);
//                 if (product != null) return product.metadata.localizedPriceString;
//                 return "";
//             }
//             catch { return ""; }
//         }

//         int productId;
//         Action<int> actDone;
//         Action<string> actFail;
//         public static void BuyProductID(int productI, Action<int> actD, Action<string> actF = null)
//         {
//             if (m_StoreController != null && m_StoreExtensionProvider != null)
//             {
//                 Instance.productId = productI;
//                 Instance.actDone = actD;
//                 Instance.actFail = actF;
//                 Product product = m_StoreController.products.WithID(IAPData.GetData(productI).Package);
//                 if (product != null && product.availableToPurchase)
//                     m_StoreController.InitiatePurchase(product);

//                 Ez.Log("Buy ID: " + productI);
//                 Ez.Log("Buy package: " + IAPData.GetData(productI).Package);
//             }
//             else Instance.InitializePurchasing();
//         }

//         public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//         {
//             bool validPurchase = true;
//             IPurchaseReceipt[] arrReceipt = null;
// #if UNITY_EDITOR
// #elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
//             var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
//             try
//             {
//                 arrReceipt = validator.Validate(args.purchasedProduct.receipt);
//                 Ez.Log("Receipt is valid. Contents:");
//                 foreach (IPurchaseReceipt productReceipt in arrReceipt)
//                 {
//                     Ez.Log(productReceipt.productID);
//                     Ez.Log(productReceipt.purchaseDate);
//                     Ez.Log(productReceipt.transactionID);
//                 }
//             }
//             catch (IAPSecurityException)
//             {
//                 Ez.Log("Invalid receipt, not unlocking content");
//                 validPurchase = false;
//             }
// #endif

//             Ez.Log("Valid Purchase " + validPurchase);
//             Ez.Log("Package " + args.purchasedProduct.definition.id);
//             Ez.Log("Transaction ID: " + args.purchasedProduct.transactionID);
//             Ez.Log("Product ID:" + productId);
//             if (IAPData.GetData(productId) != null)
//             {
//                 Ez.Log("Product " + IAPData.GetData(productId).Package);
//                 if (validPurchase && String.Equals(args.purchasedProduct.definition.id, IAPData.GetData(productId).Package, StringComparison.Ordinal))
//                 {
//                     try
//                     {
//                         actDone?.Invoke(productId);
//                         if (SaveGame.DayFirstPurchase == 0) SaveGame.DayFirstPurchase = 1;
//                         SaveGame.MoneyPaid += IAPData.GetData(productId).Cost;
//                         ManagerStorage.Save();

//                         if (arrReceipt != null && productId != -1)
//                             foreach (IPurchaseReceipt receipt in arrReceipt)
//                             {
//                                 Ez.Log(receipt.productID);
//                                 Ez.Log(receipt.purchaseDate);
//                                 Ez.Log(receipt.transactionID);

//                                 List<string> list = new List<string>();
//                                 list.Add(SaveGame.ArenaUserID);
//                                 list.Add("" + receipt.transactionID);
//                                 list.Add("" + (receipt.purchaseDate - new System.DateTime(1970, 1, 1)).TotalMilliseconds.ToString());
//                                 list.Add("" + receipt.productID);
//                                 list.Add("" + IAPData.GetData(productId).Cost);
//                                 list.Add("" + SaveGame.Lv);
//                                 ManagerNetwork.Request(ManagerNetwork.IAP_LOG, list, null);

//                                 Parameter[] para = new Parameter[2];
//                                 para[0] = new Parameter("iap_package", IAPData.GetData(productId).Package);
//                                 para[1] = new Parameter("iap_level", SaveGame.Lv);
//                                 ManagerGame.LogEvent("purchase_iap", para);
//                                 break;
//                             }
//                     }
//                     catch (Exception) { }
//                 }
//             }

//             productId = -1;
//             actDone = null;
//             actFail = null;
//             return PurchaseProcessingResult.Complete;
//         }

//         public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//         {
//             Ez.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//         }
//     }
// }