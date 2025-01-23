using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoryManager
{
    private List<HistoryLog> historyLogs;
    private const int LogsPerPage = 20;

    public HistoryManager()
    {
        historyLogs = new List<HistoryLog>();
    }

    public void AddHistory(string eventDescription, MessageTag eventType, int eventID)
    {
        HistoryLog newLog = new HistoryLog(eventDescription, eventType, eventID);
        historyLogs.Add(newLog);
        UIManager.Instance.DisplayHistory();
    }

    public List<HistoryLog> GetAllHistory()
    {
        return historyLogs;
    }

    public List<HistoryLog> GetHistoryByType(MessageTag eventType)
    {
        return historyLogs.Where(log => log.eventType == eventType).ToList();
    }

    public List<HistoryLog> GetHistoryByDateRange(DateTime start, DateTime end)
    {
        return historyLogs.Where(log => log.timestamp >= start && log.timestamp <= end).ToList();
    }

    public List<HistoryLog> GetSortedHistoryByTimestamp(bool ascending = true)
    {
        if (ascending)
        {
            return historyLogs.OrderBy(log => log.timestamp).ToList();
        }
        else
        {
            return historyLogs.OrderByDescending(log => log.timestamp).ToList();
        }
    }

    public HistoryLog? GetHistoryByID(int eventID)
    {
        return historyLogs.FirstOrDefault(log => log.eventID == eventID);
    }

    public void RemoveHistoryByID(int eventID)
    {
        historyLogs.RemoveAll(log => log.eventID == eventID);
    }

    public int GetTotalLogCount()
    {
        return historyLogs.Count;
    }

    public List<HistoryLog> GetHistoryByTagAndPage(MessageTag tag, int pageNumber)
    {
        var filteredLogs = historyLogs.Where(log => log.eventType == tag)
                                      .Skip((pageNumber - 1) * LogsPerPage)
                                      .Take(LogsPerPage)
                                      .ToList();

        return filteredLogs;
    }

    public string GetPagedTextByTag(MessageTag tag, int pageNumber)
    {
        var logs = GetHistoryByTagAndPage(tag, pageNumber);

        if (logs.Count == 0)
        {
            return $"No logs found for tag '{tag}' on page {pageNumber}.";
        }

        string pageText = string.Empty;
        foreach (var log in logs)
        {
            pageText += $"{log.ToString()}\n";
        }

        return pageText;
    }

    public List<HistoryLog> GetHistoryByIDAndPage(int eventID, int pageNumber)
    {
        var filteredLogs = historyLogs.Where(log => log.eventID == eventID)
                                      .Skip((pageNumber - 1) * LogsPerPage)
                                      .Take(LogsPerPage)
                                      .ToList();

        return filteredLogs;
    }

    public string GetPagedTextByID(int eventID, int pageNumber)
    {
        var logs = GetHistoryByIDAndPage(eventID, pageNumber);

        if (logs.Count == 0)
        {
            return $"{eventID}가 없는 페이지 입니다.";
        }

        string pageText = string.Empty;
        foreach (var log in logs)
        {
            pageText += $"{log.ToString()}\n";
        }

        return pageText;
    }

    public int GetTotalPages()
    {
        return (int)Math.Ceiling((double)historyLogs.Count / LogsPerPage);
    }

    public string GetAllHistoryAsText()
    {
        string fullHistoryText = string.Empty;
        foreach (var log in historyLogs)
        {
            fullHistoryText += $"{log.ToString()}\n";
        }
        return fullHistoryText;
    }

    public string DisplayHistory(MessageTag tags = MessageTag.전체)
    {
        string msg = string.Empty;
        if (tags != MessageTag.전체)
        {
            msg = GetPagedTextByTag(tags, 1);
        }
        else
        {
            msg = GetAllHistoryAsText();
        }
        return msg;
    }
}

public enum MessageTag
{
    전체,
    퀘스트,              // Quest-related event
    플레이어_피해,       // Player received damage
    적_피해,             // Enemy received damage
    기타,                // Other events
    플레이어_회복,       // Player healed
    플레이어_버프,       // Player received a buff
    플레이어_디버프,     // Player received a debuff
    치명타,              // Critical hit occurred
    빗나감,              // Attack missed
    회피,                // Attack dodged
    적_등장,             // Enemy spawned
    적_처치,             // Enemy defeated
    경고,                // Warning message
    오류,                // Error message
    시스템_알림,         // System notice
    이벤트,              // General event message
    아이템_획득,         // Item picked up
    아이템_사용,         // Item used up
    아이템_버림,         // Item droped
    희귀_아이템,         // Rare item obtained
    금화_획득,           // Gold acquired
    아군_회복,           // Ally healed
    아군_피해,           // Ally received damage
    팀_버프,             // Team-wide buff
    스킬_사용,           // Skill used
    마법_사용            // Magic cast
}

