# Binary Tree Traversal Application

This project demonstrates various binary tree traversal algorithms implemented in Java. The application includes classes for representing binary trees, performing different types of traversals, and practical applications of these algorithms.

## Project Structure

```
binary-tree-traversal-app
├── src
│   └── main
│       └── java
│           └── com
│               └── mahedee
│                   └── treetraversal
│                       ├── TreeNode.java
│                       ├── BinaryTree.java
│                       ├── TreeTraversalDemo.java
│                       ├── DatabaseIndexTraversal.java
│                       ├── ASTProcessor.java
│                       ├── FileSystemTraversal.java
│                       └── SceneGraphRenderer.java
├── pom.xml
└── README.md
```

## Classes Overview

- **TreeNode.java**: Defines the `TreeNode` class, representing a single node in a binary tree. It includes properties for the element, left child, right child, and parent node, along with constructors and a `toString` method.

- **BinaryTree.java**: Implements the `BinaryTree` class, which contains methods for adding nodes, checking tree properties, and performing various traversal algorithms (preorder, inorder, postorder, and level order). It also includes methods for printing the tree structure.

- **TreeTraversalDemo.java**: Serves as the entry point for the application. It demonstrates the functionality of the `BinaryTree` class by building an example tree, performing different traversals, and displaying the results.

- **DatabaseIndexTraversal.java**: Contains a class that demonstrates how to perform range queries on a B-Tree index using inorder traversal. It includes methods for executing the range query and processing the results.

- **ASTProcessor.java**: Defines a class for processing Abstract Syntax Trees (ASTs). It includes methods for generating code from expression trees using postorder traversal and pretty-printing the trees in infix notation.

- **FileSystemTraversal.java**: Provides utilities for traversing a file system. It includes methods for listing directories using preorder traversal and calculating directory sizes using postorder traversal.

- **SceneGraphRenderer.java**: Contains a class for rendering scenes in a game engine. It includes methods for rendering scene graphs using preorder traversal and detecting collisions using level-order traversal.

## Setup Instructions

1. Clone the repository:
   ```
   git clone <repository-url>
   ```

2. Navigate to the project directory:
   ```
   cd binary-tree-traversal-app
   ```

3. Build the project using Maven:
   ```
   mvn clean install
   ```

4. Run the application:
   ```
   mvn exec:java -Dexec.mainClass="com.mahedee.treetraversal.TreeTraversalDemo"
   ```

## Usage Examples

- The application demonstrates various tree traversal methods and their applications, including:
  - Copying tree structures
  - Evaluating mathematical expressions
  - Performing range queries on B-Trees
  - Rendering scenes in a game engine

## License

This project is licensed under the MIT License. See the LICENSE file for more details.