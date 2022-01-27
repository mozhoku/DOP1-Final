using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UChest_Functions : MonoBehaviour
{
    private GameObject UIElementGlo;
    // Start is called before the first frame update
    public void OpenedHealthBooster(GameObject UIElement) {
        UIElementGlo = UIElement;
        LeanTween.move(UIElementGlo.GetComponent<RectTransform>(), new Vector3(752f, 310f, 0), 1.0f).setEase(LeanTweenType.easeOutExpo).setOnComplete(RetractUIElement);
        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerEffectors.bdid_pick_up_health_booster = true;
        pc.max_health += 20;
        pc.health += 20;
    }

    void RetractUIElement() {
        LeanTween.move(UIElementGlo.GetComponent<RectTransform>(), new Vector3(1267f, 310f, 0), 1.0f).setEase(LeanTweenType.easeOutExpo).setDelay(1.5f);
    }

    public void OpenCheckHealthBooster(GameObject go) {
        if (PlayerEffectors.bdid_pick_up_health_booster) {
            Destroy(go);
        }
    }

    
    public void OpenCheckDamageBooster(GameObject go) {
        if (PlayerEffectors.bdid_get_damage_boost) {
            Destroy(go);
        }
    }

    public void GotDamageBooster(GameObject UIElement) {
        UIElementGlo = UIElement;
        LeanTween.move(UIElementGlo.GetComponent<RectTransform>(), new Vector3(752f, 310f, 0), 1.0f).setEase(LeanTweenType.easeOutExpo).setOnComplete(RetractUIElement);
        PlayerEffectors.bdid_get_damage_boost = true;
    }


}
