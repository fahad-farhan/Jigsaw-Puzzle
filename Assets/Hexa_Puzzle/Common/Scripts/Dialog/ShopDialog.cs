using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopDialog : Dialog {

	public Text[] numHintTexts, priceTexts; 

	protected override void Start ()
	{
		base.Start ();

		int i = 0;
		foreach(var numHintText in numHintTexts)
		{
			numHintText.text = Purchaser.instance.iapItems[i].value + " hints";
			i++;
		}

		i = 0;
		foreach(var priceText in priceTexts)
		{
			priceText.text = "$" + Purchaser.instance.iapItems[i].price;
			i++;
		}
	}

	public void OnBuyProduct(int index)
    {
        Sound.instance.PlayButton();
        Purchaser.instance.BuyProduct(index);
    }
}
