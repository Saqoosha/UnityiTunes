using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Controller : MonoBehaviour
{

    public Text _status;
    public Text _artist;
    public Text _album;
    public Text _title;
    public Text _duration;
    public Text _playerPosition;


    void Start()
    {
        iTunesHelper.Init();
        iTunesHelper.OnStatusChanged -= OnStatusChanged;
        iTunesHelper.OnStatusChanged += OnStatusChanged;

        _status.text = iTunesHelper.Status.ToString();
        _artist.text = "";
        _album.text = "";
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
            _playerPosition.text = iTunesHelper.PlayerPosition.ToString();
        }
    }


    public void PlayPause()
    {
        iTunesHelper.PlayPause();
    }


    public void Stop()
    {
        iTunesHelper.Stop();
    }


    public void Rewind()
    {
        iTunesHelper.Rewind();
    }


    public void SetPositionZero()
    {
        iTunesHelper.PlayerPosition = 0;
    }


    void OnStatusChanged(string status)
    {
        Debug.Log("Status: " + status);
        _status.text = status;
        _artist.text = iTunesHelper.CurrentArtist;
        _album.text = iTunesHelper.CurrentAlbum;
        _title.text = iTunesHelper.CurrentTitle;
        _duration.text = iTunesHelper.Duration.ToString();
    }


    void OnApplicationQuit()
    {
        iTunesHelper.OnStatusChanged -= OnStatusChanged;
        iTunesHelper.Cleanup();
    }

}
