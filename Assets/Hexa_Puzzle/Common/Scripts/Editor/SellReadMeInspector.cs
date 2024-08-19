#define UAS
//#define CHUPA
//#define SMA

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#pragma warning disable

[CustomEditor(typeof(SellReadMe))]
public class SellReadMeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("1. Edit Game Settings (Admob, In-app Purchase..)", EditorStyles.boldLabel);

        if (GUILayout.Button("Edit Game Settings", GUILayout.MinHeight(40)))
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Hexa_Puzzle/Common/Prefabs/GameMaster.prefab");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("2. Game Documentation", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Full Documentation", GUILayout.MinHeight(40)))
        {
			Application.OpenURL("https://docs.google.com/document/d/1OAXvZBdFssDxOnIty_rU8fnrRLK5aZVjhbwIdqFy5sQ/edit?usp=sharing");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Change Admob ids and Setup In-app Purchase Video Guide", GUILayout.MinHeight(40)))
        {
			Application.OpenURL("https://youtu.be/0Z2nKBoJGbY");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Level Editor - Add more packages and levels", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://youtu.be/rn_7UNkFzJE");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("3. My Other Great Source Codes", EditorStyles.boldLabel);

        if (products != null)
        {
            foreach (var product in products)
            {
                if (GUILayout.Button(product.name, GUILayout.MinHeight(30)))
                {
#if UAS
                    Application.OpenURL(product.uas);
#elif SMA
                    Application.OpenURL(product.sma);
#endif
                }
                EditorGUILayout.Space();
            }
        }

        EditorGUILayout.LabelField("4. Contact Us For Support", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Email: ", "phuongdong0702@gmail.com");
        EditorGUILayout.TextField("Skype: ", "phuongdong0702");
    }

    private List<MyProduct> products;
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("my_products"))
            products = JsonUtility.FromJson<MyProducts>(PlayerPrefs.GetString("my_products")).products;

        var www = new WWW("http://66.45.240.107/myproducts/superpow_products.json");
        ContinuationManager.Add(() => www.isDone, () =>
        {
            if (!string.IsNullOrEmpty(www.error)) return;
            PlayerPrefs.SetString("my_products", www.text);
            products = JsonUtility.FromJson<MyProducts>(www.text).products;

            Repaint();
        });
    }
}

[System.Serializable]
public class MyProduct
{
    public string name;
    public string uas;
    public string chupa;
    public string sma;
}

public class MyProducts
{
    public List<MyProduct> products;
}