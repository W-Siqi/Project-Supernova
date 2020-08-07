using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventDescription {
    const char PARAGRAPH_DIVIDE = ';';

    public const char CONVERT_START = '[';
    public const char CONVERT_END = ']';
    // 角色前提例子 [-c0] -代表前提，c代表charcter 0 代表bindinginfo的下标
    public const char PRECONDITON_BEGIN = '-';
    // 角色后果例子 [>c0] >代表后果，c代表charcter 0 代表bindinginfo的下标
    public const char CONSEQUENCE_BEGIN = '>';
    public const char CHARACTER_SIGN = 'c';
    // value后果例子 [>s] >代表后果，s代表statusVector
    public const char STATUS_VALUE_CHANGE_SIGN = 's';

    // 动画演出sign
    public const char ANI_START_SIGN = '(';
    public const char ANI_END_SIGN = ')';
    // 动画(c0) ,c代表character, 0 代表bindinginfo的下标
    public const char CHARACTER_ANIM_SIGN = 'c';
    // 动画(h12),h是hightlight，1 是bindinginfo，2是personalites的下标
    public const char TRAIT_HIGHTLIGHT_ANIM_SIGN = 'h';
    // 动画(t12怒),t是transfer，1 是bindinginfo，2是personalites的下标，2是tranfer的目标
    public const char TRAIT_TRANSFER_ANIM_SIGN = 't';


    public string title = "";
    public List<string> paragragh = new List<string>();

    public static EventDescription Generate(EventCard eventCard, BindingInfo[] bindingInfos) {
        var description = new EventDescription();
        description.title = eventCard.name;
        var contentStr = ParseToContentString(eventCard, bindingInfos);
        description.paragragh.AddRange(DivideToParagraphs(contentStr));
        return description;
    }

    private static string ParseToContentString(EventCard eventCard, BindingInfo[] bindingInfos) {
        var rawString = eventCard.description;
        var contentString = "";
        int cur = 0;
        while (cur < rawString.Length) {
            if (rawString[cur] != CONVERT_START) {
                contentString += rawString[cur];
                cur++;
            }
            else {
                int start = cur + 1;
                int end = start;
                while (end < rawString.Length && rawString[end] != CONVERT_END) {
                    end++;
                }
                contentString += ParseConvertSegment(rawString.Substring(start, end - start),eventCard,bindingInfos);
                cur = end + 1;
            }
        }
        return contentString;
    }

    // [convertsign] ,不包括 [ 和 ]
    private static string ParseConvertSegment(string convertSegment, EventCard eventCard, BindingInfo[] bindingInfos) {
        if (convertSegment[0] == PRECONDITON_BEGIN) {
            if (convertSegment[1] == CHARACTER_SIGN) {
                // 角色前提
                int index = convertSegment[2] - '0';
                return eventCard.preconditonSet.characterPreconditions[index].CreateDescription(bindingInfos[index],index);
            }
        }
        else if (convertSegment[0] == CONSEQUENCE_BEGIN) {
            if (convertSegment[1] == CHARACTER_SIGN) {
                // 角色后果
                int index = convertSegment[2] - '0';
                return eventCard.consequenceSet.characterConsequences[index].CreateDescription(bindingInfos[index],index);
            }
            else if(convertSegment[1] == STATUS_VALUE_CHANGE_SIGN){
                // 状态值后果
                return eventCard.consequenceSet.statusConsequence.CreateDescription();
            }
        }

        return convertSegment;
    }

    private static string[] DivideToParagraphs(string contentString) {
        List<string> paragraphs = new List<string>();
        int start = 0;
        string curStr = "";
        while (start < contentString.Length) {
            int end = start;
            while (end < contentString.Length && contentString[end] != PARAGRAPH_DIVIDE) {
                end++;
            }
            paragraphs.Add(contentString.Substring(start, end - start));
            start = end + 1;
        }
        return paragraphs.ToArray();
    }
}
