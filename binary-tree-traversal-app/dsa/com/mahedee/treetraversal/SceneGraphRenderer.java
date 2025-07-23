package com.mahedee.treetraversal;

import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

public class SceneGraphRenderer {

    /**
     * Render scene using preorder traversal
     * Parent transformations must be applied before children
     */
    public void renderScene(SceneNode node, Matrix transform) {
        // Preorder: process parent first
        Matrix nodeTransform = transform.multiply(node.getLocalTransform());

        if (node.hasGeometry()) {
            renderGeometry(node.getGeometry(), nodeTransform);
        }

        // Then process children with accumulated transform
        for (SceneNode child : node.getChildren()) {
            renderScene(child, nodeTransform);
        }
    }

    /**
     * Collision detection using level-order traversal
     * Check broad phase first, then narrow down
     */
    public List<Collision> detectCollisions(SceneNode root) {
        List<Collision> collisions = new LinkedList<>();
        Queue<SceneNode> queue = new LinkedList<>();
        queue.offer(root);

        while (!queue.isEmpty()) {
            SceneNode current = queue.poll();
            // Add collision detection logic here
            queue.addAll(current.getChildren());
        }

        return collisions;
    }
}