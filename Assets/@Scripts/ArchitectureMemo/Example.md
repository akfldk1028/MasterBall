public void UpgradeBall(int ballId, int newLevel)
{
    // DataLoader에서 공 데이터 가져오기
    BallData ballData = DataLoader.instance.GetBallData(ballId);
    
    // 레벨에 맞는 설정 가져오기
    BallLevelConfig levelConfig = ballData.GetLevelConfig(newLevel);
    
    // 공에 레벨 설정 적용
    ApplyBallLevelConfig(levelConfig);
}

private void ApplyBallLevelConfig(BallLevelConfig config)
{
    // 스킬 설정
    if (config.DefaultSkillId > 0)
    {
        SkillData skill = DataLoader.instance.SkillDic[config.DefaultSkillId];
        // 스킬 적용 로직...
    }
    
    // 물리 속성 변경
    // 시각 효과 변경
    // 기타 설정 적용...
}