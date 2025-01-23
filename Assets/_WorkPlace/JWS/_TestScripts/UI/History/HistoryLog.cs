using System;

public struct HistoryLog
{
    public DateTime timestamp;  // 이벤트 발생 시간
    public string eventDescription;  // 이벤트 내용
    public MessageTag eventType;  // 이벤트 유형(예: 전투, 상호작용 등)
    public int eventID;  // 이벤트 고유 ID (필요한 경우)

    // 생성자
    public HistoryLog(string eventDescription, MessageTag eventType, int eventID)
    {
        this.timestamp = DateTime.Now;
        this.eventDescription = eventDescription;
        this.eventType = eventType;
        this.eventID = eventID;
    }

    // 출력 포맷
    public override string ToString()
    {
        string _color = UIManager.Instance.GetColorByTag(eventType);
        return $"<mspace=0.5em>{timestamp.ToString("HH:mm:ss")}</mspace>  <color={_color}>{eventDescription}</color>";
    }
}
