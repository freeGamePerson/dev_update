--角色死亡
function onEnd(target, buff)
    if GetFightData().fight_id == 10100103 then
        AddBattleDialog(1010010301)
    end
end

--大回合开始前
function afterAllEnter(target, buff)
    if GetFightData().fight_id == 10100903 and GetFightData().round == 1 and target.side == 2 then
        AddBattleDialog(1010090301)
    end
end