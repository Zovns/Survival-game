using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FarmTool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float aimAssistRadius = 0.5f;
    public GameObject equippedItemPositionPart;
 
    public GameObject cursor;
    public float distanceToMove = 2;
    public int baseDamage = 10;
    public int range = 10;
    public string type = "Axe";
    public int level = 1;
    public AudioClip hitDenied;
    public AudioClip swingClip;
    public AudioClip chopTree;
    public float rotateOnXOnHit = 70;
    public float timeToHit = .5f;
    private AudioSource audioSource;
    private bool itemInUse = false;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ! itemInUse)
        {
            StartCoroutine(useItem());
          
        }
       
    }

    IEnumerator useItem()
    {
        itemInUse = true;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        Vector3 swingDirection = Vector3.Lerp(transform.forward, ray.direction, 0.5f).normalized;
        float startXRotation = transform.rotation.eulerAngles.x;
        
        float timePassed = 0;
        playSound(swingClip);
        while ( timePassed< timeToHit)
        {
            float progress = timePassed / timeToHit;
            Vector3 targetPosition = equippedItemPositionPart.transform.position + swingDirection * distanceToMove;
            Vector3 currentVector3 = Vector3.Slerp(equippedItemPositionPart.transform.position, targetPosition, progress);
            transform.position = currentVector3;
            float currentXRotation = startXRotation + (rotateOnXOnHit * progress);
            transform.rotation = Quaternion.Euler(new Vector3(currentXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
           
            timePassed += Time.deltaTime;
           
            yield return null;
            
        }
        float finalXRotation = startXRotation + rotateOnXOnHit;
        transform.rotation = Quaternion.Euler(new Vector3(finalXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        Vector3 finalPosition = equippedItemPositionPart.transform.position + swingDirection * distanceToMove;
        transform.position = finalPosition;
      
        startXRotation = transform.rotation.eulerAngles.x;

       StartCoroutine(detectHit());
      

        timePassed = 0;
        while (timePassed < timeToHit)
        {
            float progress = timePassed /timeToHit;
            Vector3 targetPosition = equippedItemPositionPart.transform.position + swingDirection * distanceToMove;
            Vector3 currentVector3 = Vector3.Slerp(targetPosition,equippedItemPositionPart.transform.position, progress);
            transform.position = currentVector3;
            float currentXRotation = startXRotation - (rotateOnXOnHit * progress);
            transform.rotation = Quaternion.Euler(new Vector3(currentXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
          
            timePassed += Time.deltaTime;
            yield return null;
        }
        finalXRotation = startXRotation - rotateOnXOnHit;
        transform.rotation = Quaternion.Euler(new Vector3(finalXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        transform.position = equippedItemPositionPart.transform.position;
        itemInUse = false;
    }

    void playSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    IEnumerator detectHit()
    {
        Debug.Log("called");
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        // Create a ray from the camera through the center of the screen


        // Store raycast hit info
        RaycastHit hit;

        // Perform the raycast
        if (!Physics.Raycast(ray, out hit, range + level))
        {
            Physics.SphereCast(ray, aimAssistRadius, out hit, range + level);

        }


        //hit.collider.gameObject.pa
        GameObject parentObject = hit.collider?.gameObject.transform.parent?.gameObject;
        
        if (parentObject == null)
        {
            Debug.Log("no hit");
           yield break;
        }
        DamageableObject damageableObject = parentObject.GetComponent<DamageableObject>();
        if (damageableObject == null)
        {
            Debug.Log(parentObject.name);
            yield break;
        }
        Debug.Log("hit");
        if (parentObject.CompareTag("Farmable"))
        {

            if ((damageableObject.type == "Tree" && type == "Axe") || (damageableObject.type == "Rock" && type == "Pickaxe"))
            {
                if (level < damageableObject.level)
                {
                    playSound(hitDenied);
                }
                else
                {
                    float damage = baseDamage + (level - damageableObject.level) * baseDamage;
                    playSound(chopTree);
                    damageableObject.TakeDamage(damage);
                    yield return new WaitForSeconds(.1f);
                }
            }
            else
            {
                playSound(hitDenied);
            }

        }
        else if(level >= damageableObject.level){
            if (parentObject.name == "Sheep")
            {
                SheepAI sheepAI = parentObject.GetComponent<SheepAI>();
                sheepAI.Scare();

            }
            float damage = baseDamage + (level - damageableObject.level) * baseDamage;
            damageableObject.TakeDamage(damage);
        }
        Debug.DrawLine(ray.origin, hit.point, Color.red, 2f); // For visualization
    }
}
