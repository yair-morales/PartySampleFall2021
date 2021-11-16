using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
    /* need 2 values: max boost amount, current boost amount
     * need recovery/recharge rate of boost juice
     * need rate of boost discharge when boosting
     * need prefabs for ui that we can change
     * 
     * 
     */

    public BasicVehicleInputHandler basicVehicleInputHandler;
    public float maxBoostAmount;
    public float currentBoostAmount;
    public float boostRechargeRate;
    public float boostDischargeRate;
    public Slider boostUI;
    public float nitroBoostAmount;

    // Start is called before the first frame update
    void Start()
    {
        //currentBoostAmount = maxBoostAmount;
        boostUI.maxValue = maxBoostAmount;
        //boostUI.value = maxBoostAmount;
    }

    // Update is called once per frame
    void Update()
    {
        boostUI.value = currentBoostAmount;
    }

    public void RechargeBoost()
    {
        currentBoostAmount += boostRechargeRate * Time.deltaTime;
        currentBoostAmount = Mathf.Clamp(currentBoostAmount, 0, maxBoostAmount);
    }

    public void DischargeBoost()
    {
        currentBoostAmount -= boostDischargeRate * Time.deltaTime;
        currentBoostAmount = Mathf.Clamp(currentBoostAmount, 0, maxBoostAmount);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nitro")) {
            currentBoostAmount += nitroBoostAmount;
            Destroy(collision.gameObject);
            Debug.Log("collided");
        }
    }
}


