package com.mahedee.treetraversal;

import java.util.ArrayList;
import java.util.List;

public class TreeTraversalDemo {
    
    public static void main(String[] args) {
        System.out.println("BINARY TREE TRAVERSAL ALGORITHMS DEMO");
        System.out.println("Author: Mahedee Hasan");

        System.out.println("═".repeat(50));
        
        // Build example tree
        BinaryTree<String> tree = buildExampleTree();
        
        // Show tree structure
        tree.printTreeStructure();
        System.out.println();
        
        // Demonstrate all traversals
        demonstrateTraversals(tree);
        
        // Show practical applications
        demonstratePracticalApplications();
    }
    
    private static BinaryTree<String> buildExampleTree() {
        // Building this tree:
        //       A
        //      / \
        //     B   C
        //    / \   \
        //   D   E   F
        //  /
        // G
        
        BinaryTree<String> tree = new BinaryTree<>();
        
        TreeNode<String> a = tree.addRoot("A");
        TreeNode<String> b = tree.addLeft(a, "B");
        TreeNode<String> c = tree.addRight(a, "C");
        TreeNode<String> d = tree.addLeft(b, "D");
        TreeNode<String> e = tree.addRight(b, "E");
        TreeNode<String> f = tree.addRight(c, "F");
        tree.addLeft(d, "G");
        
        return tree;
    }
    
    private static void demonstrateTraversals(BinaryTree<String> tree) {
        System.out.println("TRAVERSAL RESULTS:");
        System.out.println("─".repeat(40));
        
        // Preorder traversal
        List<String> preorder = tree.preorderTraversal();
        System.out.println("Preorder  (Root→Left→Right): " + preorder);
        System.out.println("Use case: Tree copying, directory listing");

        // Inorder
        List<String> inorder = tree.inorderTraversal();
        System.out.println("🟩 Inorder   (Left→Root→Right): " + inorder);
        System.out.println("   💡 Use case: BST sorted output, expression trees");
        
        // Postorder
        List<String> postorder = tree.postorderTraversal();
        System.out.println("🟨 Postorder (Left→Right→Root): " + postorder);
        System.out.println("   💡 Use case: Tree deletion, folder size calculation");
        
        // Level Order
        List<String> levelOrder = tree.levelOrderTraversal();
        System.out.println("🟪 Level Order (Top→Bottom): " + levelOrder);
        System.out.println("   💡 Use case: BFS, shortest path, tree printing");
        
        System.out.println();
    }
    
    private static void demonstratePracticalApplications() {
        System.out.println("🎯 PRACTICAL APPLICATIONS:");
        System.out.println("─".repeat(40));
        
        // BST example
        demonstrateBSTSorting();
        
        // Expression tree example
        demonstrateExpressionEvaluation();
        
        // File system example
        demonstrateFileSystemOperations();
    }
    
    private static void demonstrateBSTSorting() {
        System.out.println("📊 Binary Search Tree Sorting:");
        
        BinaryTree<Integer> bst = new BinaryTree<>();
        TreeNode<Integer> root = bst.addRoot(50);
        TreeNode<Integer> left = bst.addLeft(root, 30);
        TreeNode<Integer> right = bst.addRight(root, 70);
        bst.addLeft(left, 20);
        bst.addRight(left, 40);
        bst.addLeft(right, 60);
        bst.addRight(right, 80);
        
        System.out.println("   Input BST: [50, 30, 70, 20, 40, 60, 80]");
        System.out.println("   Inorder:   " + bst.inorderTraversal());
        System.out.println("   ✅ Automatically sorted!");
        System.out.println();
    }
    
    private static void demonstrateExpressionEvaluation() {
        System.out.println("🧮 Expression Tree Evaluation:");
        System.out.println("   Expression: (3 + 5) * 2");
        System.out.println("   Inorder:    3 + 5 * 2     (infix notation)");
        System.out.println("   Postorder:  3 5 + 2 *     (postfix notation)");
        System.out.println("   ✅ Different traversals = Different notations!");
        System.out.println();
    }
    
    private static void demonstrateFileSystemOperations() {
        System.out.println("📁 File System Operations:");
        System.out.println("   Preorder:   List directories before contents");
        System.out.println("   Postorder:  Calculate sizes (children first)");
        System.out.println("   Level Order: Show hierarchy level by level");
        System.out.println("   ✅ Each traversal serves different purposes!");
        System.out.println();
    }
}