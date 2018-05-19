using Pipliz;
using UnityEngine;

namespace AdvancedWand
    {

    public static class AdvancedWandHelper
        {

        //Misc checks to call a command
        public static bool CheckCommand(Players.Player player, string arg, int expected_args, out string[] args)
            {
            args = null;
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
                Pipliz.Chatting.Chat.Send(player, "Wand is OFF, use //wand to activate");
                return false;
                }

            //Pos1 initialized
            if(null == wand.pos1)
                {
                Pipliz.Chatting.Chat.Send(player, "Pos 1 not initialized");
                return false;
                }

            //Pos2 initialized
            if(null == wand.pos2)
                {
                Pipliz.Chatting.Chat.Send(player, "Pos 2 not initialized");
                return false;
                }

            args = ChatCommands.CommandManager.SplitCommand(arg);

            if(expected_args != args.Length)
                {
                Pipliz.Chatting.Chat.Send(player, "Wrong Arguments");
                return false;
                }

            return true;
            }

        //pos1 & pos2 MUST be initialized
        public static void GenerateCorners(Players.Player player, out Vector3Int start, out Vector3Int end)
            {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            start = Vector3Int.Min(wand.pos1, wand.pos2);
            end = Vector3Int.Max(wand.pos1, wand.pos2);
            }

        private static bool CheckBlock(Players.Player player, ushort block)
            {
            if(block == 0)  //Air block
                return true;

            ItemTypes.ItemType type = ItemTypes.GetType(block);

            if(!type.IsPlaceable || type.NeedsBase || !ItemTypes.NotableTypes.Contains(type))
                {
                Pipliz.Chatting.Chat.Send(player, "You can't place this block");
                return false;
                }

            return true;
            }

        public static bool GetBlockIndex(Players.Player player, string block, out ushort blockIndex)
            {
            blockIndex = 0;

            try
                {
                bool isNumeric = int.TryParse(block, out int IDBlock);

                if(isNumeric)
                    blockIndex = (ushort)IDBlock;
                else
                    blockIndex = ItemTypes.IndexLookup.GetIndex(block);
                }
            catch(System.ArgumentException)
                {
                Pipliz.Chatting.Chat.Send(player, "Block not found");
                return false;
                }

            //Check if you can use this block
            return CheckBlock(player, blockIndex);
            }

        public static bool CheckLimit(Players.Player player)
            {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            int x = Math.Abs(wand.pos1.x - wand.pos2.x) + 1; //+1 is to take into account the start block
            int y = Math.Abs(wand.pos1.y - wand.pos2.y) + 1;
            int z = Math.Abs(wand.pos1.z - wand.pos2.z) + 1;

            int blocks_in_selected_area = x * y * z;


            if(wand.limit < blocks_in_selected_area)
                {
                Pipliz.Chatting.Chat.Send(player, string.Format("You are trying to change {0} and the limit is {1}. You can change the limit with //limit <new_limit>", blocks_in_selected_area, wand.limit));
                return false;
                }

            return true;

            }

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
