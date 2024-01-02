using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TirednessLives : MonoBehaviour
{
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;

    GameObject[] tirednessImages;

    public void Initialize() {
        tirednessImages = new GameObject[GameManager.Instance.maxLives];

        for (int i = 0; i < tirednessImages.Length; i++) {
            tirednessImages[i] = new GameObject("Tiredness Image");
            tirednessImages[i].transform.parent = this.transform;
            tirednessImages[i].transform.localScale = new Vector3(0.5f,0.5f,1f);

            tirednessImages[i].gameObject.AddComponent<Image>();
            tirednessImages[i].GetComponent<Image>().sprite = openSprite;
            tirednessImages[i].transform.position = this.transform.position + new Vector3(i,0,0);
        }
    }

    public void UpdateLives(int currentLives) {

        for (int i = 0; i < tirednessImages.Length; i++) {
            tirednessImages[i].GetComponent<Image>().sprite = openSprite; //yeah yeah whatever. get it done.
        }

        for (int z = tirednessImages.Length - 1;
            z >= currentLives;
            z--) {
            tirednessImages[z].GetComponent<Image>().sprite = closedSprite;
        }
    }
}
