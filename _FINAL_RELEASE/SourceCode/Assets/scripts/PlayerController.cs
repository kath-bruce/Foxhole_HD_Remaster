using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    AudioSource audioSrc;

    public AudioClip walkingOnGrass;
    public AudioClip walkingOnWood;

    float init_y;
    float jump_speed = 50f;
    const float jump_time_reset = 0.5f;
    float jump_time = jump_time_reset;
    float max_y_vel = 10f;

    float turning_rate = 10f;
    float movement_multiplier = 30f;

    bool inAir = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();

        init_y = gameObject.transform.position.y;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Water")
        {
            //death - respawn instantly
            GameController.INSTANCE.WaterCollision();
        }
        else if (other.gameObject.name == "LevelEnd")
        {
            //finished level
            GameController.INSTANCE.LevelComplete();
        }

        if (other.gameObject.name == "Terrain")
        {
            audioSrc.clip = walkingOnGrass;

            //landed on object so no longer in the air - can reset jump timer
            inAir = false;
            jump_time = jump_time_reset;
        }
        else if (other.gameObject.name.Substring(0, 5) == "Crate")
        {
            audioSrc.clip = walkingOnWood;

            //landed on object so no longer in the air - can reset jump timer
            inAir = false;
            jump_time = jump_time_reset;
        }
    }

    //note Update instead of FixedUpdate means that pause can be set quicker and the timer is more accurate 
    //this is a runner, so this is important 
    void Update()
    {
        //Time.delta_time is used a lot here so cache it
        float delta_time = Time.deltaTime;

        //below values are multiples of time.deltatime so needs a multiplier so the player moves in a reasonable manner
        float ad = (Input.GetAxis("Horizontal") * movement_multiplier);
        float ws = (Input.GetAxis("Vertical") * movement_multiplier);

        float jump = Input.GetAxis("Jump");

        Vector3 playerVec = gameObject.transform.eulerAngles;

        #region directional vectors (limited to 0 - 360)
        //note not sure if directional vectors need to be limited

        Vector3 rightVec = Camera.main.transform.eulerAngles;
        Vector3 leftVec = Camera.main.transform.eulerAngles;
        Vector3 frontVec = Camera.main.transform.eulerAngles;
        Vector3 backVec = Camera.main.transform.eulerAngles;

        if (rightVec.y > 270)
        {
            float below360 = 360 - rightVec.y;
            float above360 = 90 - below360;
            rightVec.y = above360;
        }
        else
        {
            rightVec.y += 90;
        }

        if (leftVec.y < 90)
        {
            float above360 = 90 - leftVec.y;
            float below360 = 360 - above360;
            leftVec.y = below360;
        }
        else
        {
            leftVec.y -= 90;
        }

        if (backVec.y > 180)
        {
            backVec.y -= 180;
        }
        else
        {
            backVec.y += 180;
        }

        #endregion

        #region jumping

        if (!GameController.INSTANCE.GamePaused())
        {
            rb.WakeUp();

            //if player just let go of space bar
            if (Input.GetKeyUp(KeyCode.Space)) //note assumes space bar is assigned to jump
            {
                //this will disallow double jumping
                jump_time = 0;
            }

            //if player is jumping and has not reached the limit on how long the player can jump
            if (jump > 0 && jump_time > 0 && rb.velocity.y >= 0)
            {
                //decrease current remaining jump time
                jump_time -= delta_time;

                rb.AddForce((Vector3.up * jump_speed) * delta_time, ForceMode.Impulse);

                //limit on how fast the player can jump
                if (rb.velocity.y > max_y_vel)
                {
                    Vector3 max_vel = new Vector3();
                    max_vel.y = max_y_vel;
                    rb.velocity = max_vel;
                }

                //player is currently in the air
                inAir = true;
            }
            else if (inAir || rb.velocity.y < 0) //if player is not jumping or has reached jump limit but is still in the air, player is falling
            {
                rb.AddForce((Vector3.down * jump_speed * jump_speed) * delta_time);
                //fall_time += delta_time;
            }
        }
        else
        {
            rb.Sleep();
        }

        #region animations
        //set relevant animations
        if (inAir || GameController.INSTANCE.GamePaused())
        {
            //note fox fbx does not contain jumping animation
            anim.enabled = false;
        }
        else if (ad != 0f || ws != 0f) //if player is otherwise moving
        {
            anim.enabled = true;
            anim.Play("RUN");
        }
        else
        {
            //player is not moving - idle
            anim.enabled = true;
            anim.Play("IDLE");
        }
        #endregion

        #endregion

        #region rotation and movement

        //note why does movement only work on last axis?

        if (!GameController.INSTANCE.GamePaused())
        {
            bool isPlayerMoving = false;

            if (ad > 0) //player is turning and moving right
            {
                gameObject.transform.rotation = Quaternion.SlerpUnclamped(gameObject.transform.rotation, Quaternion.Euler(rightVec), turning_rate * delta_time);

                gameObject.transform.Translate(new Vector3(0, 0, ad * delta_time));

                isPlayerMoving = true;
            }
            else if (ad < 0) //player is turning and moving left
            {
                gameObject.transform.rotation = Quaternion.SlerpUnclamped(gameObject.transform.rotation, Quaternion.Euler(leftVec), turning_rate * delta_time);

                gameObject.transform.Translate(new Vector3(0, 0, -ad * delta_time));

                isPlayerMoving = true;
            }

            if (ws > 0) //player is turning and moving forward/away from the camera
            {
                gameObject.transform.rotation = Quaternion.SlerpUnclamped(gameObject.transform.rotation, Quaternion.Euler(frontVec), turning_rate * delta_time);

                gameObject.transform.Translate(new Vector3(0, 0, ws * delta_time));

                isPlayerMoving = true;
            }
            else if (ws < 0) //player is turning and moving towards the camera
            {
                gameObject.transform.rotation = Quaternion.SlerpUnclamped(gameObject.transform.rotation, Quaternion.Euler(backVec), turning_rate * delta_time);

                gameObject.transform.Translate(new Vector3(0, 0, -ws * delta_time));

                isPlayerMoving = true;
            }

            if (isPlayerMoving && !audioSrc.isPlaying)
            {
                audioSrc.Play();
            }
            else if (!isPlayerMoving || inAir)
            {
                audioSrc.Stop();
            }
        }
        else
        {
            audioSrc.Stop();
        }

        #endregion

        #region collision detection failure hack

        //hack to sort out occasional failure in collision detection with ground
        //it actually happens a lot?????? maybe because this method is no longer FixedUpdate

        //note assumes player starts on lowest bit of terrain

        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(gameObject.transform.position, Vector3.down);

        if (!Physics.Raycast(ray, out hit))
        {
            Vector3 good_vec = gameObject.transform.position;
            good_vec.y = init_y;
            gameObject.transform.position = good_vec;

            //Debug.Log("Collision detection with Terrain failed");
        }

        #endregion
    }
}
