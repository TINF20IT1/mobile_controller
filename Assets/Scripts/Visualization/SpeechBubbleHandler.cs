using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleHandler : MonoBehaviour
{
    public GameObject speechBubbles;
    void Update()
    {
        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.GetChild(i).position);
        }
    }

    public GameObject show(string a)
    {
        if(transform.Find(a) != null) return null;
        Vector2 pos = Random.insideUnitCircle * 0.05f;
        GameObject go = Instantiate(speechBubbles,Vector3.zero, Quaternion.identity, transform);
        go.name = a;
        go.transform.localPosition = new Vector3(pos.x,0.01f, pos.y);
        go.GetComponentInChildren<TextMesh>().text = a;

        return go;
    }

    public void hide(GameObject go)
    {
        Destroy(go.gameObject);
    }

    public void display(string a, float time = 2)
    {
        StartCoroutine(display_coroutine(a,time));
    }
    IEnumerator display_coroutine(string a, float time = 2)
    {
        GameObject go = show(a);
        yield return new WaitForSeconds(time);
        hide(go);
    }
}
