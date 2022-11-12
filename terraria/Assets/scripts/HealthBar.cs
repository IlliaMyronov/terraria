using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    private List<List<GameObject>> hearts;
    private int healthPerHeart;
    [SerializeField]
    private GameObject heartPrefab;
    private void Awake()
    {
        hearts = new List<List<GameObject>>();
        healthPerHeart = 20;
    }

    // Start is called before the first frame update
    void Start()
    {
        hearts.Add(new List<GameObject>());
        int numHearts = character.GetComponent<Character>().getHealth()/healthPerHeart;
        for (int i = 0; i < numHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab) as GameObject;
            heart.transform.position = new Vector2((3*i)+35,30);
            hearts[0].Add(heart);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            character.GetComponent<Character>().setHealth(character.GetComponent<Character>().getHealth() + healthPerHeart);
            this.addHeart();
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            character.GetComponent<Character>().setHealth(character.GetComponent<Character>().getHealth() - healthPerHeart);
            this.removeHeart();
        }
    }

    private void addHeart()
    {
        
        int lastElement = hearts.Count - 1;
        GameObject heart = Instantiate(heartPrefab) as GameObject;
        if (hearts[lastElement].Count == 5)
        {
            hearts.Add(new List<GameObject>());
            lastElement++;
            heart.transform.position = new Vector2(35, 30 - (3 * (hearts.Count - 1)));
        }
        else
        {
            heart.transform.position = new Vector2((3 * hearts[lastElement].Count) + 35, 30 - (3 * (hearts.Count - 1)));
        }
        hearts[lastElement].Add(heart);
    }

    private void removeHeart()
    {
        int lastElement = hearts.Count - 1;
        Debug.Log(hearts[lastElement].Count - 1);
        Destroy(hearts[lastElement][hearts[lastElement].Count - 1]);
        hearts[lastElement].RemoveAt(hearts[lastElement].Count - 1);
        if (hearts[lastElement].Count == 0)
        {
            hearts.RemoveAt(lastElement);
        }
    }
    
}
