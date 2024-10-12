using TMPro;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Animations;
using System.Net.NetworkInformation;




// using UnityEngine.Random;
//10月19日締め切り
public class SampleCodeText : MonoBehaviour
{
    // A~Zのアルファベットリストを作成
    List<char> SampleCode = new List<char>();
    public TextMeshProUGUI SampleText;

    void Start()
    {
        // アルファベットのリストを作成
        for (char Code = 'a'; Code <= 'z'; Code++)
        {
            SampleCode.Add(Code);
        }
        
        Randomjudge();
        

    }

    void Randomjudge()
    {
        int randomIndex1 = Random.Range(0, 26);
        int randomIndex2 = Random.Range(0, 26);
        int randomIndex3 = Random.Range(0, 26);
        int randomIndex4 = Random.Range(0, 26);
        int randomIndex5 = Random.Range(0, 26);

        char randomLetter1 = SampleCode[randomIndex1];// ランダム数字とListの番号と比較
        char randomLetter2 = SampleCode[randomIndex2];
        char randomLetter3 = SampleCode[randomIndex3];
        char randomLetter4 = SampleCode[randomIndex4];
        char randomLetter5 = SampleCode[randomIndex5];
        
        Debug.Log("選ばれた文字は: " + randomLetter1 + " と " + randomLetter2 + " と " + randomLetter3 + " と " + randomLetter4 + " と " + randomLetter5);//確認用

        SampleText.text = randomLetter1.ToString() + randomLetter2.ToString() +randomLetter3.ToString() +randomLetter4.ToString() +randomLetter5.ToString() ;
        // Listのアルファベットを出力
        Debug.Log(SampleText);
        }
}