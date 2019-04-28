using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour {

    public float hbound = 1;
    public float vbound = 1;

    private int hpMax = 10;
    private int vpMax = 10;
    private int hp = 0;
    private int vp = 0;
    private int money = 0;
    private float score = 0;
    private bool gameover = false;


    private bool singing = false;
    private float singtime = 0.0f;
    private float singMaxTime = 1.0f;

    private Rigidbody2D rbd2d;
    private GameObject singProgressBar;

    public GameObject hpProgress;
    public GameObject hungerProgress;
    public GameObject scoreText;
    public GameObject lifeText;
    public GameObject upgradePanel;
    public GameObject gameOverPanel;

    public GameObject bulletPrefab;
    public GameObject bitePrefab;

    private float vpTime = 0.0f;
    private float playerSpeed = 1.0f;
	// Use this for initialization
	void Start () {
        rbd2d = gameObject.GetComponent<Rigidbody2D>();
        singProgressBar = transform.Find("TopCanvas").Find("ProgressBar").gameObject;
        hp = hpMax;
        vp = vpMax;
        gameOverPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (gameover) {
            return;
        }
        CheckTimeTick();
        VpTick();
        OnPlayerInput();
        UpdateAnimation();
        UpdateHUD();
	}

    void UpdateAnimation() {
        float velocity = rbd2d.velocity.magnitude;
        gameObject.GetComponent<Animator>().SetFloat("velocity", velocity);

    }

    void OnPlayerInput() {
        if (singing) {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            SkillPack();
        }else if(Input.GetKeyUp(KeyCode.E)) {
            SkillEat();
        }else if (Input.GetMouseButtonDown(0)) {
            SkillShoot();
        }else if (Input.GetMouseButtonDown(1)) {
            SkillBite();
        }else if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.Escape)) {
            if (upgradePanel.activeSelf) {
                upgradePanel.SetActive(false);
            } else {
                upgradePanel.SetActive(true);
            }
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 position = transform.position + new Vector3(h, v, 0);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(h, v) * playerSpeed);

    }
    void UpdateUpgradePanel(int[] stockList) {
        upgradePanel.GetComponent<UpgradeCtrl>().UpdateInfo(money, stockList);
    }
    private int[] priceList = new int[]{1, 2, 2, 3, 2, 5};
    private string[] nameList = new string[] { "heal", "speed", "shoot_power", "bite_power", "eat_time", "bind_time"};
    private int[] stockList = new int[] {-1, 2, 2, 1, 1, 1};
    public void Buy(int index) {
        string name = nameList[index];
        int stock = stockList[index];
        int price = priceList[index];
        if(stock == 0) {
            return;
        }
        if(money < price) {
            return;
        }
        money -= price;
        if (name.Equals("heal")) {
            hp = hp + 4 > hpMax ? hpMax : hp + 4;
        } else if (name.Equals("speed")) {
            playerSpeed += 0.5f;
            stockList[index] -= 1;
        } else if (name.Equals("shoot_power")) {
            bulletDam += 1;
            stockList[index] -= 1;
        } else if (name.Equals("bite_power")) {
            biteDam += 2;
            stockList[index] -= 1;
        } else if (name.Equals("eat_time")) {
            eatTime -= 0.5f;
            stockList[index] -= 1;
        } else if (name.Equals("bind_time")) {
            bindTime -= 1.0f;
            stockList[index] -= 1;
        }
        UpdateUpgradePanel(stockList);
    }

    void CheckTimeTick() {
        float delta = Time.deltaTime;
        CheckScoreTick();
        if (singing && (singtime > 0.0f)) {
            singtime -= delta;
            if(singtime < 0) {
                singtime = 0.0f;
                singing = false;
                singSuccessCB();
                singProgressBar.SetActive(false);
            } else {
                singProgressBar.GetComponent<Progress>().progress = (singMaxTime - singtime) / singMaxTime;
            }
        }
    }

    private GameObject FindTarget() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float neardis = 10000f;
        foreach (GameObject target in targets) {
            float dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < neardis) {
                neardis = dis;
                nearest = target;
            }
        }
        if(neardis > 1.0f) {
            return null;
        }
        return nearest;
    }

    void CheckScoreTick() {
        score += Time.deltaTime;
    }

    void VpTick() {
        float delta = Time.deltaTime;
        vpTime += delta;
        if(vpTime < 3.0f) {
            return;
        }
        vpTime = 0.0f;

        if(vp == 0) {
            OnDamage(1, false);
        }else {
            vp -= 1;
        }
    }

    private int bulletDam = 1;
    private int biteDam = 3;
    void SkillShoot() {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 from = new Vector3(transform.position.x, transform.position.y, 0.0f);
        Vector3 to = new Vector3(mousePoint.x, mousePoint.y, 0.0f);
        Vector3 force = (to - from).normalized;
        var angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg - 90.0f;
        GameObject nBullet = Instantiate(
            bulletPrefab,
            transform.position + force * 0.5f,
            Quaternion.AngleAxis(angle, Vector3.forward)
            );
        nBullet.GetComponent<Rigidbody2D>().AddForce(force * 0.4f);
        nBullet.GetComponent<BulletCtrl>().dam = bulletDam;

    }

    void SkillBite() {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 from = new Vector3(transform.position.x, transform.position.y, 0.0f);
        Vector3 to = new Vector3(mousePoint.x, mousePoint.y, 0.0f);
        Vector3 force = (to - from).normalized;
        Vector3 bitePos = transform.position + force * 0.5f;
        GameObject nBullet = Instantiate(
            bitePrefab,
            bitePos,
            Quaternion.identity
        );
        var collider2Ds = Physics2D.OverlapCircleAll(
            new Vector2(bitePos.x, bitePos.y),
            1.0f,
            LayerMask.GetMask("enemy")
            );
        if (collider2Ds.Length > 0) {
            var collider0 = collider2Ds[0];
            collider0.gameObject.GetComponent<EnemyCtrl>().OnBite(biteDam);
        }

        
    }

    private System.Action singSuccessCB;
    private System.Action singFailCB;
    void StartSing(float time, System.Action success, System.Action fail) {
        singing = true;
        singtime = time;
        singMaxTime = time;
        singSuccessCB = success;
        singFailCB = fail;
        singProgressBar.SetActive(true);

    }

    void BreakSing() {
        singing = false;
        singtime = 0.0f;
        if (singFailCB != null) {
            singFailCB();
            singProgressBar.SetActive(false);
        }
        singFailCB = null;
    }

    private float bindTime = 2.0f;
    void SkillPack() {
        GameObject target = FindTarget();
        if (target == null) {
            return;
        }
        EnemyCtrl enemy = target.GetComponent<EnemyCtrl>();
        if (!enemy.paralysised) {
            return;
        }
        StartSing(bindTime, SkillPackSuccess, SkillPackFailed);
        Destroy(target);
    }

    void SkillPackSuccess() {
        money += 1;
    }

    void SkillPackFailed() {
    }

    private float eatTime = 1.0f;
    void SkillEat() {
        GameObject target = FindTarget();
        if (target == null) {
            return;
        }
        EnemyCtrl enemy = target.GetComponent<EnemyCtrl>();
        if (!enemy.paralysised) {
            return;
        }
        StartSing(eatTime, SkillEatSuccess, SkillEatFailed);
        Destroy(target);
    }

    void SkillEatSuccess() {
        vp += 4;
        if(vp > vpMax) {
            vp = vpMax;
        }
    }

    void SkillEatFailed() {

    }

    void OnDamage(int dam, bool breakSing) {
        hp = hp - dam;
        if(hp <= 0) {
            hp = 0;
            GameOver();
        }
        if (breakSing) {
            BreakSing();
        }
    }

    void GameOver() {
        gameover = true;
        gameOverPanel.transform.Find("ScoreText").gameObject.GetComponent<Text>().text = Mathf.FloorToInt(score).ToString();
        gameOverPanel.SetActive(true);
    }

    void OpenShop() {
        upgradePanel.SetActive(true);
    }

    void CloseShop() {
        upgradePanel.SetActive(false);
    }

    void UpdateHUD() {
        hpProgress.GetComponent<Progress>().progress = (hp + 0.0f) / hpMax;
        hungerProgress.GetComponent<Progress>().progress = (vp + 0.0f) / vpMax;
        scoreText.GetComponent<Text>().text = Mathf.FloorToInt(score).ToString();
        lifeText.GetComponent<Text>().text = Mathf.FloorToInt(money).ToString();
        UpdateUpgradePanel(stockList);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            var enemey = collision.gameObject.GetComponent<EnemyCtrl>();
            if (!enemey.paralysised) {
                OnDamage(2, true);
            }
        }
    }

    public void Restart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
