package com.mahedee.treetraversal;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

public class BinaryTree<E> {
    private TreeNode<E> root;  
    private int size;          
    
    public BinaryTree() {
        this.root = null;
        this.size = 0;
    }
    
    public TreeNode<E> addRoot(E element) throws IllegalStateException {
        if (root != null) {
            throw new IllegalStateException("Tree already has a root");
        }
        root = new TreeNode<>(element);
        size++;
        return root;
    }
    
    public TreeNode<E> addLeft(TreeNode<E> parent, E element) {
        if (parent.left != null) {
            throw new IllegalArgumentException("Node already has a left child");
        }
        TreeNode<E> child = new TreeNode<>(element, parent);
        parent.left = child;
        size++;
        return child;
    }
    
    public TreeNode<E> addRight(TreeNode<E> parent, E element) {
        if (parent.right != null) {
            throw new IllegalArgumentException("Node already has a right child");
        }
        TreeNode<E> child = new TreeNode<>(element, parent);
        parent.right = child;
        size++;
        return child;
    }
    
    public TreeNode<E> getRoot() { return root; }
    public int size() { return size; }
    public boolean isEmpty() { return size == 0; }
    
    public List<E> preorderTraversal() {
        List<E> result = new ArrayList<>();
        preorderHelper(root, result);
        return result;
    }
    
    private void preorderHelper(TreeNode<E> node, List<E> result) {
        if (node != null) {
            result.add(node.element);                    
            preorderHelper(node.left, result);          
            preorderHelper(node.right, result);         
        }
    }
    
    public List<E> inorderTraversal() {
        List<E> result = new ArrayList<>();
        inorderHelper(root, result);
        return result;
    }
    
    private void inorderHelper(TreeNode<E> node, List<E> result) {
        if (node != null) {
            inorderHelper(node.left, result);           
            result.add(node.element);                   
            inorderHelper(node.right, result);          
        }
    }
    
    public List<E> postorderTraversal() {
        List<E> result = new ArrayList<>();
        postorderHelper(root, result);
        return result;
    }
    
    private void postorderHelper(TreeNode<E> node, List<E> result) {
        if (node != null) {
            postorderHelper(node.left, result);         
            postorderHelper(node.right, result);        
            result.add(node.element);                   
        }
    }
    
    public List<E> levelOrderTraversal() {
        List<E> result = new ArrayList<>();
        if (root == null) return result;
        
        Queue<TreeNode<E>> queue = new LinkedList<>();
        queue.offer(root);
        
        while (!queue.isEmpty()) {
            TreeNode<E> current = queue.poll();
            result.add(current.element);                
            
            if (current.left != null) {
                queue.offer(current.left); 
            }
            if (current.right != null) {
                queue.offer(current.right); 
            }
        }
        return result;
    }
    
    public void printTreeStructure() {
        System.out.println("🌳 Tree Structure:");
        if (root == null) {
            System.out.println("(empty tree)");
            return;
        }
        printHelper(root, "", true);
    }
    
    private void printHelper(TreeNode<E> node, String prefix, boolean isLast) {
        if (node != null) {
            System.out.println(prefix + (isLast ? "└── " : "├── ") + node.element);
            
            boolean hasLeft = node.left != null;
            boolean hasRight = node.right != null;
            
            if (hasLeft) {
                printHelper(node.left, prefix + (isLast ? "    " : "│   "), !hasRight);
            }
            if (hasRight) {
                printHelper(node.right, prefix + (isLast ? "    " : "│   "), true);
            }
        }
    }
}