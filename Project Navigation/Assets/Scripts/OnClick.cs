using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int objectId;
    [SerializeField] private string objectType;
    [SerializeField] private Text textField;

    // Canvas
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject button;

    private string baseURL = "http://localhost:7777/api/";

    private void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (objectType != "joystick")
        {
            TaskOnClick();
            StartCoroutine(GetDataAtIndex());
        }
      
    }

    private void TaskOnClick()
    {
        panel.SetActive(true);
        button.SetActive(true);
    }

    private IEnumerator GetDataAtIndex()
    {
        string URL = baseURL + objectType + "/" + objectId.ToString();

        UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL);

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.LogError(unityWebRequest.error);
            yield break;
        }

        JSONNode jsonData = JSON.Parse(unityWebRequest.downloadHandler.text);
        //RadiatorData radiatorData = ParseRadiatorData(jsonData);
        PipeData pipeData = ParsePipeData(jsonData);

        //DisplayData(radiatorData);
        DisplayPipeData(pipeData);
    }

    private PipeData ParsePipeData(JSONNode jsonData)
    {
        PipeData pipeData = new PipeData();

        pipeData.id_tube = jsonData["id_tube"].AsInt;
        pipeData.num_auditorium = jsonData["num_auditorium"];
        pipeData.id_rizer = jsonData["id_rizer"].AsInt;
        pipeData.length = jsonData["length"].AsDouble;
        pipeData.diameter = jsonData["diameter"].AsInt;
        pipeData.material = jsonData["material"];
        pipeData.temperature = jsonData["temperature"].AsInt;
        pipeData.type_name = jsonData["type_name"];

        return pipeData;
    }
    private void DisplayPipeData(PipeData pipeData)
    {
        textField.text =
            $"Информация о трубе \n" +
            $" \n" +
            $"ID: {pipeData.id_tube} \n" +
            $"Стояк отопления: {pipeData.id_rizer} \n" +
            $"Длина: {pipeData.length} \n" +
            $"Диаметр: {pipeData.diameter} \n" +
            $"Материал: {pipeData.material} \n" +
            $"Допустимая температура теплоносителя в трубах: {pipeData.temperature} \n" +
            $"Тип: {pipeData.type_name}";
    }

    private RadiatorData ParseRadiatorData(JSONNode jsonData)
    {
        RadiatorData radiatorData = new RadiatorData();

        radiatorData.num_auditorium = jsonData["num_auditorium"];
        radiatorData.id_rizer = jsonData["id_rizer"].AsInt;
        radiatorData.type = jsonData["type"];
        radiatorData.floor = jsonData["floor"];
        radiatorData.model = jsonData["model"];
        radiatorData.quantity_section = jsonData["quantity_section"].AsInt;
        radiatorData.power = jsonData["power"].AsInt;
        radiatorData.data_begin = jsonData["data_begin"];
        radiatorData.data_end = jsonData["data_end"];
        radiatorData.material = jsonData["material"].AsObject;

        return radiatorData;
    }


    private void DisplayRadiatirData(RadiatorData radiatorData)
    {
        textField.text = 
            $"Аудитория: {radiatorData.num_auditorium} \n" +
            $"Стояк: {radiatorData.id_rizer} \n" +
            $"Тип: {radiatorData.type} \n" +
            $"Этаж: {radiatorData.floor} \n" +
            $"Модель: {radiatorData.model} \n" +
            $"Кол-во секций: {radiatorData.quantity_section} \n" +
            $"Теплоотдача: {radiatorData.power} \n" +
            $"Дата начала эксплуатации: {radiatorData.data_begin} \n" +
            $"Дата окончания эксплуатации: {radiatorData.data_end} \n" +
            $"Материал: {radiatorData.material}";
    }
}

[System.Serializable]
public class RadiatorData
{
    public int num_auditorium;
    public int id_rizer;
    public string type;
    public string floor;
    public string model;
    public int quantity_section;
    public int power;
    public string data_begin;
    public string data_end;
    public object material;
}

[System.Serializable]
public class PipeData
{
    public int id_tube;
    public string num_auditorium;
    public int id_rizer;
    public double length;
    public int diameter;
    public string material;
    public int temperature;
    public string type_name;
}