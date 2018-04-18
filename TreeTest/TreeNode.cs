using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeNode
    {
        public readonly InputEnum ID;

        public List<TreeNode> Children = new List<TreeNode>();
        
        public QuestionList QuestionsToAsk { get; set; }

        public TreeNode(InputEnum id)
        {
            this.ID = id;
        }

        public static QuestionList FindQuesions(
            TreeNode node,
            Account dbResults,
            Account userInput,
            List<InputEnum> path,
            QuestionList questions
        )
        {
            //log step
            if (node.ID != InputEnum.Root)
            { path.Add(node.ID); }

            //get children
            var children = node.Children;
            if (children.Any())
            {
                foreach (var child in children)
                {
                    //recursively itterate down to the next node
                    questions = FindQuesions(
                        child,
                        dbResults,
                        userInput,
                        path, 
                        questions
                    );

                    //check if last iteration yielded any results
                    if(questions.Questions.Any())
                    { break; }
                    else
                    {
                        //remove last node to make path accurate
                        path.RemoveAt(path.Count - 1);

                        //continue to process all children
                        if (children.IndexOf(child) < children.Count - 1)
                        { continue; }

                        //if node has no questions move to next node
                        if (ReferenceEquals(null, node.QuestionsToAsk))
                        { continue; }

                        //before heading back up the tree, check nodes condition if node has questions
                        if (node.QuestionsToAsk.Questions.Any())
                        {
                            //check if user input matches db reusults based on property datapoints as defined by the branch path
                            var matchFound = Account.AreEqual(dbResults, userInput, path);
                            if (matchFound)
                            {
                                questions = node.QuestionsToAsk;
                                break;
                            }
                        }
                    }
                }
            }
            //node is last in its branch
            else
            {
                //check if user input matches db reusults based on property datapoints as defined by the branch path
                var matchFound = Account.AreEqual(dbResults, userInput, path);
                if (matchFound)
                { questions = node.QuestionsToAsk; }
            }

            return questions;
        }//end FindQuesions
    }
}


