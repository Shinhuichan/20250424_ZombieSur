using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using NaughtyAttributes;
public class DBManager : MonoBehaviour
{
    public enum DBLocationType
    {
        Local,
        Web, //미구현
        SQL, //미구현
    }
    public enum DataFormatType
    {
        XML,
        JSON,
        CSV
    }
    [SerializeField] DBLocationType selectLocation;
    [SerializeField] DataFormatType selectFormat;
    [SerializeField] string fileName;

    [System.Serializable]
    public class Example1
    {
        public List<Example2> GunDatas = new List<Example2>();
    }
    [System.Serializable]
    public class Example2
    {
        public string Name;
        public float Damage;
        public float TimeBetFire;
        public float reloadTime;
        public int MagCapacity;
        public int StartAmmo;
        public string ShotclipName;
        public string reloadClipName;
    }
    public Example1 cash;
    [Button]
    public void Load()
    {
        if(selectFormat == DataFormatType.JSON)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(fileName + "(JSON)");
            if(textAsset == null)
            {
                Debug.Log("그런 파일은 찾지 못했습니다");
                return;
            }
            cash = JsonUtility.FromJson<Example1>(textAsset.text);
        }
        else if(selectFormat == DataFormatType.CSV)
        {
            //미구현
        }
        else if(selectFormat == DataFormatType.XML)
        {
            string str = "(XML)";
            // string path = Application.persistentDataPath + "/" + fileName + str + ".txt";
            // Debug.Log($"{Application.persistentDataPath} /// {path}");
            // if (!File.Exists(path))
            // {
            //     Debug.Log("그런 파일은 찾지 못했습니다!");
            //     return;
            // }
            TextAsset textAsset = Resources.Load<TextAsset>(fileName + str);
            if(textAsset == null)
            {
                Debug.Log("그런 파일은 찾지 못했습니다");
                return;
            }
            // string xmlContent = File.ReadAllText(path);
            string xmlContent = textAsset.text;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            XmlNodeList nodes = xmlDoc.SelectNodes("GunDatas/GunData");       
            cash = new Example1();
            cash.GunDatas = new List<Example2>();
            // 해당 노드의 character 노드의 내용을 loop문으로 전부 출력
            // 현재의 내용이라면 1회만 돌리고 탈출함. -> character 노드가 1개뿐이어서
            for(int i=0; i< nodes.Count; i++)
            {
                Example2 example2 = new Example2();
                example2.Name = nodes[i].SelectSingleNode("Name").InnerText; 
                example2.Damage = float.Parse(nodes[i].SelectSingleNode("Damage").InnerText);
                example2.TimeBetFire = float.Parse(nodes[i].SelectSingleNode("TimeBetFire").InnerText);
                example2.reloadTime = float.Parse(nodes[i].SelectSingleNode("reloadTime").InnerText);
                example2.MagCapacity = int.Parse(nodes[i].SelectSingleNode("MagCapacity").InnerText);
                example2.StartAmmo = int.Parse(nodes[i].SelectSingleNode("StartAmmo").InnerText);
                example2.ShotclipName = nodes[i].SelectSingleNode("ShotclipName").InnerText;
                example2.reloadClipName = nodes[i].SelectSingleNode("reloadClipName").InnerText;
                cash.GunDatas.Add(example2);
            }  
        }
    }
    [Button]
    public void Save()
    {
        if(selectFormat == DataFormatType.JSON)
        {
            string dataString = JsonUtility.ToJson(cash);
            string name = fileName;
#if UNITY_EDITOR
            string path = "Assets/Temp/";
            Directory.CreateDirectory("Assets/Temp");
#else
            string path = Application.persistentDataPath + "/";
#endif
            string str = "(JSON)";
            File.WriteAllText(path + fileName + str, dataString);
        }
        else if(selectFormat == DataFormatType.CSV)
        {

            //미구현

        }
        else if(selectFormat == DataFormatType.XML)
        {

            // XML 파일 저장을 위한 XmlDocument 생성
            XmlDocument xmlDoc = new XmlDocument();
            
            // 루트 노드 생성
            XmlElement root = xmlDoc.CreateElement("GunDatas");
            xmlDoc.AppendChild(root);

            // GunDatas에 각 GunData 추가
            foreach (var gunData in cash.GunDatas)
            {
                XmlElement gunDataElement = xmlDoc.CreateElement("GunData");

                // 각 속성 추가
                gunDataElement.AppendChild(CreateElement(xmlDoc, "Name", gunData.Name));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "Damage", gunData.Damage.ToString()));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "TimeBetFire", gunData.TimeBetFire.ToString()));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "reloadTime", gunData.reloadTime.ToString()));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "MagCapacity", gunData.MagCapacity.ToString()));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "StartAmmo", gunData.StartAmmo.ToString()));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "ShotclipName", gunData.ShotclipName));
                gunDataElement.AppendChild(CreateElement(xmlDoc, "reloadClipName", gunData.reloadClipName));

                root.AppendChild(gunDataElement);
            }

            // 파일 경로 설정
            string str = "(XML)";
            string path = Application.persistentDataPath + "/" + fileName + str + ".txt";
            
            // XML 파일 저장
            xmlDoc.Save(path);

            Debug.Log($"XML 파일이 저장되었습니다: {path}");


        }

    }





    // XML 요소 생성 헬퍼 메서드
    private XmlElement CreateElement(XmlDocument doc, string name, string value)
    {
        XmlElement element = doc.CreateElement(name);
        element.InnerText = value;
        return element;
    }



}
