package com.mahedee.treetraversal;

import java.util.ArrayList;
import java.util.List;

/**
 * DatabaseIndexTraversal demonstrates how to perform range queries on a B-Tree index using inorder traversal.
 */
public class DatabaseIndexTraversal {

    /**
     * Range query using inorder traversal.
     * This method simulates a range query on a B-Tree index.
     *
     * @param root The root node of the B-Tree.
     * @param start The starting key of the range.
     * @param end The ending key of the range.
     * @return A list of records that fall within the specified range.
     */
    public List<Record> rangeQuery(BTNode root, int start, int end) {
        List<Record> results = new ArrayList<>();
        rangeQueryHelper(root, start, end, results);
        return results;
    }

    private void rangeQueryHelper(BTNode node, int start, int end, List<Record> results) {
        if (node == null) return;

        // Inorder traversal naturally gives sorted order
        if (node.key > start) {
            rangeQueryHelper(node.left, start, end, results);
        }

        if (node.key >= start && node.key <= end) {
            results.add(node.record);
        }

        if (node.key < end) {
            rangeQueryHelper(node.right, start, end, results);
        }
    }
}