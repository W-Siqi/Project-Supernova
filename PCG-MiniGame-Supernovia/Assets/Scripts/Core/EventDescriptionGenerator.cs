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
    // 动画(h[charaIndex][trait-转ASCII]),h是hightlight
    public const char TRAIT_HIGHTLIGHT_ANIM_SIGN = 'h';
    // 动画(r[charaIndex][trait-转ASCII]),r是remove trait
    public const char TRAIT_REMOVE_ANIM_SIGN = 'r';
    // 动画(a[charaIndex][trait-转ASCII]),a是add trait
    public const char TRAIT_ADD_ANIM_SIGN = 'a';


    public string title = "";
    public List<string> paragragh = new List<string>();

    public static EventDescription Generate(GameState gameState, EventCard eventCard, BindingInfo[] bindingInfos) {
        Debug.Log(eventCard.name+ "  -[描述转化]  "+eventCard.description);
        var description = new EventDescription();
        description.title = eventCard.name;
        var contentStr = ParseToContentString(gameState, eventCard, bindingInfos);
        description.paragragh.AddRange(DivideToParagraphs(contentStr));
        return description;
    }

    private static string ParseToContentString(GameState gameState, EventCard eventCard, BindingInfo[] bindingInfos) {
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
                contentString += ParseConvertSegment(gameState, rawString.Substring(start, end - start),eventCard,bindingInfos);
                cur = end + 1;
            }
        }
        return contentString;
    }

    // [convertsign] ,不包括 [ 和 ]
    private static string ParseConvertSegment(GameState gameState, string convertSegment, EventCard eventCard, BindingInfo[] bindingInfos) {
        if (convertSegment[0] == PRECONDITON_BEGIN) {
            if (convertSegment[1] == CHARACTER_SIGN) {
                // 角色前提
                Debug.Log("[解析角色前提: ] " + convertSegment + " idnex为:" + (convertSegment[2] - '0'));
                int index = convertSegment[2] - '0';
                return eventCard.preconditonSet.characterPreconditions[index].CreateDescription(gameState, bindingInfos[index].bindedCharacterIndex);
            }
        }
        else if (convertSegment[0] == CONSEQUENCE_BEGIN) {
            if (convertSegment[1] == CHARACTER_SIGN) {
                // 角色后果
                Debug.Log("[解析后果前提: ] " + convertSegment + " idnex为:" + (convertSegment[2] - '0'));
                int index = convertSegment[2] - '0';
                return eventCard.consequenceSet.characterConsequences[index].CreateDescription(gameState, bindingInfos[index].bindedCharacterIndex);
            }
            else if(convertSegment[1] == STATUS_VALUE_CHANGE_SIGN){
                // 状态值后果
                Debug.Log("[解析状态值后果: ] " + convertSegment );
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
