Common_UnitConsumeActPoint(attacker, 1);
if Common_Break_Skill(attacker, _Skill) then return end
Common_ChangeEp(attacker, -_Skill.skill_consume_ep)

local target, all_targets = Common_GetTargets(...)

Common_FireWithoutAttacker(1100331, {attacker}, {Type = 30})
Common_Sleep(attacker, 0.5)

Common_ShowCfgFlagEffect(_Skill)
Common_UnitPlayAttack(attacker, _Skill.id);

Common_ShowCfgStageEffect(_Skill)
OtherEffectInCfg(attacker, target, _Skill)

Common_FireBullet(0, attacker, target, _Skill, {})
Common_ChangeHp(attacker, -attacker.hp)

Common_Sleep(attacker, 0.5)
