using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Controller : MonoBehaviour
{

    public Text _status;
    public Text _artist;
    public Text _title;
    public Text _duration;
    public Text _playerPosition;


    void Start()
    {
        iTunesHelper.Init();
        iTunesHelper.OnStatusChanged -= OnStatusChanged;
        iTunesHelper.OnStatusChanged += OnStatusChanged;

        _status.text = "";
        _artist.text = "";
        _title.text = "";
        _duration.text = "";
        _playerPosition.text = "";

        StartCoroutine(UpdateInfo());
    }


    IEnumerator UpdateInfo()
    {
        while (true)
        {
            // Debug.Log(Time.time);
            yield return new WaitForEndOfFrame();
            _artist.text = iTunesHelper.CurrentArtist;
            _title.text = iTunesHelper.CurrentTitle;
            _duration.text = iTunesHelper.Duration.ToString();
            _playerPosition.text = iTunesHelper.PlayerPosition.ToString();
        }
    }


    void OnStatusChanged(string status)
    {
        Debug.Log("Status: " + status);
        _status.text = status;
    }
    

    void OnApplicationQuit()
    {
        iTunesHelper.OnStatusChanged -= OnStatusChanged;
        iTunesHelper.Cleanup();
    }

}
