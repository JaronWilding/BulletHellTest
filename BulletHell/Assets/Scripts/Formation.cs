using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Formation : MonoBehaviour
{

    public Vector2 gridSize = new Vector2(10, 2);
    public Vector2 gridOffset = new Vector2(1f, 1f);
    public int div = 4;
    public List<Vector3> gridList = new List<Vector3>();

    public float maxMoveOffset = 5f;
    public float speed = 1f;
    
    private float curPosX;
    private Vector3 startPos;
    private int dir = -1;

    public bool canSpread;
    public bool spreadStarted;

    public float spreadAmount = 1f;
    public float spreadSpeed = 0.5f;
    private float curSpread;
    private int spreadDir = 1;

    public List<EnemyFormation> enemyFormation = new List<EnemyFormation>();

    private bool canDive;
    public List<GameObject> divePath = new List<GameObject>();

    [System.Serializable]
    public class EnemyFormation
    {
        public int e_Index;
        public float e_xPos;
        public float e_yPos;
        public GameObject e_Enemy;
        public Vector3 e_Goal;
        public Vector3 e_Start;

        public EnemyFormation(int _index, float _xPos, float _yPos, GameObject _enemy)
        {
            e_Index = _index;
            e_xPos = _xPos;
            e_yPos = _yPos;
            e_Enemy = _enemy;

            e_Start = new Vector3(_xPos, _yPos, 0f);
            e_Goal = new Vector3(_xPos + (_xPos * 0.3f), _yPos, 0f);
        }
    }
    

    private void Start()
    {
        startPos = transform.position;
        curPosX = transform.position.x;
        CreateGrid();
    }

    private void Update()
    {
        if(!canSpread && !spreadStarted)
        {
            curPosX += speed * dir * Time.deltaTime;
            if (curPosX >= maxMoveOffset)
            {
                dir *= -1;
                curPosX = maxMoveOffset;
            }
            else if (curPosX <= -maxMoveOffset)
            {
                dir *= -1;
                curPosX = -maxMoveOffset;
            }
            transform.position = new Vector3(curPosX, startPos.y, startPos.z);
        }
        if (canSpread)
        {
            curSpread += spreadDir * spreadSpeed * Time.deltaTime;
            if(curSpread >= spreadAmount || curSpread <= 0)
            {
                spreadDir *= -1;
            }

            for (int ii = 0; ii < enemyFormation.Count; ii++)
            {
                if(Vector3.Distance(enemyFormation[ii].e_Enemy.transform.position, enemyFormation[ii].e_Goal) >= 0.001f)
                {
                    enemyFormation[ii].e_Enemy.transform.position = Vector3.Lerp(transform.position + enemyFormation[ii].e_Start, transform.position + enemyFormation[ii].e_Goal, curSpread);
                }
            }
        }

        //if (canDive)
        //{
        //    Invoke("SetDiving", Random.Range(3f, 10f));
        //    canDive = false;
        //}
    }

    // Spreading method

    public IEnumerator ActivateSpread()
    {
        if (spreadStarted)
        {
            yield break;
        }
        spreadStarted = true;

        while(transform.position.x != startPos.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            yield return null;
        }
        canSpread = true;
        //canDive = true;
        Invoke("SetDiving", Random.Range(3f, 10f));
    }


    private void SetDiving()
    {
        if(enemyFormation.Count > 0)
        {
            int chosenPath = Random.Range(0, divePath.Count);
            int chosenEnemy = Random.Range(0, enemyFormation.Count);

            GameObject newPath = Instantiate(divePath[chosenPath], enemyFormation[chosenEnemy].e_Start + transform.position, Quaternion.identity);
            enemyFormation[chosenEnemy].e_Enemy.GetComponent<EnemyBehaviour>().DiveSetup(newPath.GetComponent<Path>());
            enemyFormation.RemoveAt(chosenEnemy);
            Invoke("SetDiving", Random.Range(3f, 10f));
        }
        else
        {
            CancelInvoke("SetDiving");
            return;
        }
        
    }


    // Grid work

    private void CreateGrid()
    {
        gridList.Clear();

        int num = 0;

        for (int ii = 0; ii < gridSize.x; ii++)
        {
            for (int jj = 0; jj < gridSize.y; jj++)
            {
                float x = (gridOffset.x + gridOffset.x * 2 * (num / div)) * Mathf.Pow(-1f, num % 2 + 1);// * ii;
                float y = gridOffset.y * ((num % div) / 2);

                Vector3 gridPos = new Vector3(x, y, 0f);
                
                num++;
                gridList.Add(gridPos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        int num = 0;
        CreateGrid();
        foreach(Vector3 pos in gridList)
        {
            Gizmos.DrawWireSphere(GetVector(num), 0.2f);
            num++;
        }
    }


    public Vector3 GetVector(int _ID)
    {
        return transform.position + gridList[_ID];
    }
}