using Pipliz;
using UnityEngine;

namespace AdvancedWand.Helper
{
    public static class CommandHelper
    {
        //Misc checks to call a command
        public static bool CheckCommand(Players.Player player)
        {
            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return false;

            //Player has permission
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wand is OFF, use //wand to activate</color>");
                return false;
            }

            //Pos1 initialized
            if(!wand.area.IsPos1Initialized())
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Pos 1 not initialized</color>");
                return false;
            }

            //Pos2 initialized
            if(!wand.area.IsPos2Initialized())
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Pos 2 not initialized</color>");
                return false;
            }

            return true;
        }

        //player != null
        private static bool CheckBlock(Players.Player player, ushort block)
        {
            if(0 == block)  //Air block
                return true;

            if(!ItemTypes.TryGetType(block, out ItemTypes.ItemType type))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Block not found</color>");
                return false;
            }

            if(!type.IsPlaceable || type.NeedsBase || !ItemTypes.NotableTypes.Contains(type))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>You can't place this block</color>");
                return false;
            }

            return true;
        }

        //player != null
        public static bool GetBlockIndex(Players.Player player, string block, out ushort blockIndex)
        {
            blockIndex = 0;

            if(int.TryParse(block, out int IDBlock))
                blockIndex = (ushort)IDBlock;
            else if(!ItemTypes.IndexLookup.TryGetIndex(block, out blockIndex))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Block not found</color>");
                return false;
            }

            //Check if you can use this block
            return CheckBlock(player, blockIndex);
        }

        //player != null
        public static bool CheckLimit(Players.Player player)
        {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            int blocks_in_selected_area = wand.area.GetSize();

            if(wand.limit < blocks_in_selected_area)
            {
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=red>You are trying to change {0} and the limit is {1}. You can change the limit with //limit <new_limit></color>", blocks_in_selected_area, wand.limit));
                return false;
            }

            return true;

        }

        //Thanks to Zun for this method
        public static Vector3Int GetDirection(Vector3 playerForward, string direction)
        {
            Vector3 testVector;
            // testVector will be the "local" player direction intended. It rotates as the player rotates
            switch(direction)
            {
                default:
                case "forward":
                case "front":
                case "f":
                testVector = playerForward;
                break;
                case "backward":
                case "back":
                case "b":
                testVector = -playerForward;
                break;
                case "right":
                case "r":
                testVector = -Vector3.Cross(playerForward, Vector3.up);
                break;
                case "left":
                case "l":
                testVector = Vector3.Cross(playerForward, Vector3.up);
                break;
                case "up":
                case "u":
                //testVector = Vector3.up;
                return new Vector3Int(0, 1, 0);
                break;
                case "down":
                case "d":
                //testVector = Vector3.down;
                return new Vector3Int(0, -1, 0);
                break;
            }

            float testVectorMax = Math.MaxMagnitude(testVector);
            // testVectorMax holds either x, y or z from testVector; whichever has the largest absolute value
            if(testVectorMax == testVector.x)
            {
                // so the largest part of the direction is the x-axis
                if(testVectorMax >= 0f)
                    return new Vector3Int(1, 0, 0);
                else
                    return new Vector3Int(-1, 0, 0);
            }
            /*else if(testVectorMax == testVector.y)
                {
                if(testVectorMax >= 0f)
                    return new Vector3Int(0, 1, 0);
                else
                    return new Vector3Int(0, -1, 0);
                }*/
            else
            {
                if(testVectorMax >= 0)
                    return new Vector3Int(0, 0, 1);
                else
                    return new Vector3Int(0, 0, -1);
            }
        }
    }
}
