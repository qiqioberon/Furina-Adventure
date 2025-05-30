using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
   [SerializeField] private List<Skill> skills = new List<Skill>();
   
   public List<Skill> Skills { get => skills; }

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

   public void AddSkill(Skill skill){
        skills.Add(skill);
    }
    public void RemoveSkill(Skill skill){
        skills.Remove(skill);
    }
    public void UseSkill(int index){
        Skill skill = skills[index];
        Player player = GetComponent<Player>();
        if(player.Mana >= skill.ManaCost){
            if(skill.OnCooldown) {
                UIManager.Instance.AddMessage($"{skill.SkillName} is on cooldown", "#FF0000");
                return;
            }
            
            skill.Use();
            player.Mana -= skill.ManaCost;
            StartCoroutine(skill.CooldownRoutine());
        }
        else{
            UIManager.Instance.AddMessage("Not enough mana", "#FF0000");
        }
    }

    private void Update(){
        for(int i = 0; i < skills.Count; i++){
            if(skills[i].OnCooldown){
                UIManager.Instance.UpdateCooldown(i, skills[i].RemainingCooldown);
            }
        }
    }
}
