// This file defines the TreeNode class, which represents a single node in a binary tree.
// It contains properties for the element stored in the node, as well as references to the left child, right child, and parent node.
// It includes constructors for creating a new node and an overridden toString method for easy printing of node data.

package com.mahedee.treetraversal;

public class TreeNode<E> {
    E element;              // The data stored in this node
    TreeNode<E> left;       // Points to the left child
    TreeNode<E> right;      // Points to the right child
    TreeNode<E> parent;     // Points to the parent (optional)
    
    // Create a new node with data
    public TreeNode(E element) {
        this.element = element;
        this.left = null;
        this.right = null;
        this.parent = null;
    }
    
    // Create a new node with data and parent reference
    public TreeNode(E element, TreeNode<E> parent) {
        this.element = element;
        this.parent = parent;
        this.left = null;
        this.right = null;
    }
    
    // Override toString for easy printing of node data
    @Override
    public String toString() {
        return element.toString();
    }
}