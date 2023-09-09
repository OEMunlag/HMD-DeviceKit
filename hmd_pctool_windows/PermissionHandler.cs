using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace hmd_pctool_windows;

internal class PermissionHandler
{
	private static PermissionHandler instance = new PermissionHandler();

	private Action<bool> mCallback;

	private List<RSACryptoService> cryptoServices = new List<RSACryptoService>();

	private string[] prikeys = new string[4] { "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" };

	private string[] pubkeys = new string[4] { "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1yuYRl1ZnB61tbF0CM+d1UGP0TvsnX0IYmEkafXBqXD1h28ViHwd0ZNPnH6RBCiks+DhcH9ageiiZWdWywrA6oT0nAQ8UlsmNXsx9pLsV6KSqHDBEuF2ZOeohw2gdoeLvfOE3AFfM8zfeaI+s9/bNChiFIMCudm+ggDQ2U12fqWAo6FkcZC8f3z2eQOHR1VbcYZRiVC+sZvcKt2dgRvwwTM+AYbzfXRxMjPlkDgQh7445BTXXLjtaE28DW2Ud34KHP2116jU6Cg8ezhoFQOoqXwE7MZIgyGV6g093CxbiBtnXr81++pWg6rHMG0zpKkdsZk0bZgTOVXPQZxJpWKh2wIDAQAB", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA9afchuNY6z9gqxQF+RITMEjIODx5tkvar+jodCmWQteBIW1o3gzFoXNLp2IlAyl/bQ+bOS9Rkgemm3ALzT+c0R+lWTbKxtRVSdi3yHYtqZV+b8ZdLiqFQDjKnPsobRx57w8ZP70WVVpWJWa1vTw6TdLjxMuFTSoNL5lfFoJdTkCfCi1YQ6lQOf6oT4iN4Yuvt0bGMIo0n9kQeU75ZtY1PuVlkzcCNwSS57PEnmRK6vce1z3Lv3JgFEJnydnX+kPl5Ym6IetJCwB2uHL9sipcEgMZ/W9ied2Xztj3fla38mJR2c6yc3ApolRHB2Ia7cPOuw2g5MlHcx9DNMg6NQfu4QIDAQAB", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAq69xEltz0/58p1lNus2G72EKetC8MVEinVy+F+ovSXiclxUXrdDiLdzNAod2n2KL8LWo5tkFY6MnMEF/WwAH7wq6HF+Xb26y1osysv2LAIIvQ/LRSJ+2+0p/0bzQBQaAr/jnoAByUJzU87/jheY4AtOxRA4BP3RIdGbMUxEOXZkje421wvxXAGgii3W8WgvD0+Lf5eZuArxGdO4bHtxPSPxr4tCts29WwRmRJz+4AvMLC8tl4t0tMkXpyzJB9DBQQjiiPMLtOcAjtXB56HYToaUq6lmPBynJCBOtoNI8+fP/RdjCFbY4IqLXAt/PHQOxTkqImMAMDoFjvqwX/Pm0uwIDAQAB", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxUTn9HoPCzsGcTfdfGqHq7p2HCLPg5CJMDfIV5niv/uXIxSAtxamH1eKTTFVXyyiorBVnSJxKO5ruV6DL2I7RI6tC8h3wEwCDxftz0zkwEQge2lHz64/S5urlNeM+bp/ywZi1C6c8oO/MIt0hQgEF9yGaeijTkWNhjVzJDyr/owV3uPsdLSRorlWnc8WziHlQpagmoXSXfQWkiNMMF6pWV9gvsAjVYjQwTGPENbU5jgsm6Rsz8U+YxWvNk8IyOOinMXD65Re/b1a+f5b4wIKCM7kZ5UK7topHob5BBzGGicotSpjoPPkEkCvIk9fjH2Ywa4figw6dZezrxeVyL0RPwIDAQAB" };

	public static PermissionHandler getInstance()
	{
		return instance;
	}

	public PermissionHandler()
	{
		for (int i = 0; i < prikeys.Length; i++)
		{
			cryptoServices.Add(new RSACryptoService(prikeys[i], pubkeys[i]));
		}
	}

	private string decrypt(string chiperNounce)
	{
		return cryptoServices[3].Decrypt(chiperNounce);
	}

	private string encrpt(HmdPermissionType pType, string plainText)
	{
		return cryptoServices[(int)pType].Encrypt(plainText);
	}

	private string sign(HmdPermissionType pType, string plainText)
	{
		return cryptoServices[(int)pType].PrivateEncryption(plainText);
	}

	private string getChiperResponse(HmdPermissionType pType, string serialNo, string cipherNounce)
	{
		string plainText = decrypt(cipherNounce.ToString()) + ";" + serialNo;
		StringBuilder stringBuilder = new StringBuilder("");
		HmdPcToolApi.getSecurityVersion(stringBuilder);
		if (!int.TryParse(stringBuilder.ToString(), out var result) || result < 2)
		{
			return encrpt(pType, plainText);
		}
		if (result > 2)
		{
			throw new Exception("Unsupported security version");
		}
		return sign(pType, plainText);
	}

	public bool RequestPermissionAsync(HmdPermissionType pType, Action<bool> callback)
	{
		if (mCallback != null || callback == null)
		{
			return false;
		}
		mCallback = callback;
		BackgroundWorker backgroundWorker = new BackgroundWorker();
		backgroundWorker.DoWork += bw_RequestPermission;
		backgroundWorker.RunWorkerCompleted += bw_RequestPermissionCompleted;
		backgroundWorker.RunWorkerAsync(pType);
		return true;
	}

	private void bw_RequestPermission(object sender, DoWorkEventArgs e)
	{
		HmdPermissionType hmdPermissionType = (HmdPermissionType)e.Argument;
		try
		{
			RequestPermission(hmdPermissionType);
		}
		catch (Exception ex)
		{
			string text = "";
			switch (hmdPermissionType)
			{
			case HmdPermissionType.Repair:
				text = "repair";
				break;
			case HmdPermissionType.Simlock:
				text = "simlock";
				break;
			}
			e.Result = false;
			MessageBox.Show("123 PH: Fail to get " + text + " Permission\nReason : " + ex.Message);
			return;
		}
		e.Result = true;
	}

	private void bw_RequestPermissionCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		bool obj = (bool)e.Result;
		if (mCallback != null)
		{
			mCallback(obj);
			mCallback = null;
		}
	}

	public void RequestPermission(HmdPermissionType pType)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		if (HmdPcToolApi.authStart(stringBuilder) != 0)
		{
			throw new Exception("Fail to auth start");
		}
		StringBuilder stringBuilder2 = new StringBuilder(32);
		if (HmdPcToolApi.getVar("serialno", stringBuilder2) != 0)
		{
			throw new Exception("Fail to get serailNo");
		}
		StringBuilder stringBuilder3 = new StringBuilder(32);
		if (HmdPcToolApi.getSecurityVersion(stringBuilder3) != 0)
		{
			throw new Exception("Fail to get Security version");
		}
		ServerResponse serverResponse;
		if (Program.isOfflineVersion())
		{
			serverResponse = new ServerResponse();
			serverResponse.ChiperResponse = getChiperResponse(pType, stringBuilder2.ToString(), stringBuilder.ToString());
			if (string.IsNullOrEmpty(serverResponse.ChiperResponse))
			{
				serverResponse.IsSuccessed = false;
				serverResponse.FailReason = "Failed to get device permission.";
			}
			else
			{
				serverResponse.IsSuccessed = true;
			}
		}
		else
		{
			serverResponse = AzureNativeClient.Instance.GetChiperResponse(stringBuilder.ToString(), stringBuilder2.ToString(), pType, stringBuilder3.ToString());
		}
		if (!serverResponse.IsSuccessed)
		{
			throw new Exception(serverResponse.FailReason);
		}
		if (HmdPcToolApi.requestPermission(pType, serverResponse.ChiperResponse) != 0)
		{
			throw new Exception("Fail reported by LK");
		}
	}
}
