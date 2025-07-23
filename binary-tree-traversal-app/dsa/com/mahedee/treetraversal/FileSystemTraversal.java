// FileSystemTraversal.java
package com.mahedee.treetraversal;

import java.io.File;

public class FileSystemTraversal {
    
    /**
     * Directory listing using preorder traversal
     * Shows directories before their contents
     */
    public void listDirectory(File dir, int depth) {
        // Preorder: visit directory first
        System.out.println("  ".repeat(depth) + "📁 " + dir.getName());
        
        File[] children = dir.listFiles();
        if (children != null) {
            for (File child : children) {
                if (child.isDirectory()) {
                    listDirectory(child, depth + 1);  // Recurse into subdirs
                } else {
                    System.out.println("  ".repeat(depth + 1) + "📄 " + child.getName());
                }
            }
        }
    }
    
    /**
     * Calculate directory sizes using postorder traversal
     * Must calculate children sizes before parent
     */
    public long calculateSize(File dir) {
        long totalSize = 0;
        
        File[] children = dir.listFiles();
        if (children != null) {
            for (File child : children) {
                if (child.isDirectory()) {
                    totalSize += calculateSize(child); // Recurse into subdirs
                } else {
                    totalSize += child.length(); // Add file size
                }
            }
        }
        
        // Process current directory after children (postorder)
        System.out.println(dir.getName() + ": " + totalSize + " bytes");
        return totalSize;
    }
}