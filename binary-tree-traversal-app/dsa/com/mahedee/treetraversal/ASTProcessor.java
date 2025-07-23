public class ASTProcessor {
    
    /**
     * Code generation using postorder traversal
     * Children (operands) must be processed before parent (operator)
     */
    public String generateCode(ASTNode node) {
        if (node.isLeaf()) {
            return "LOAD " + node.value;
        }
        
        // Postorder: left, right, root
        String leftCode = generateCode(node.left);
        String rightCode = generateCode(node.right);
        String opCode = "APPLY " + node.operator;
        
        return leftCode + "\n" + rightCode + "\n" + opCode;
    }
    
    /**
     * Pretty printing using inorder traversal
     * Produces human-readable infix notation
     */
    public String prettyPrint(ASTNode node) {
        if (node.isLeaf()) {
            return node.value.toString();
        }
        
        // Inorder: left, root, right
        return "(" + prettyPrint(node.left) + " " + 
               node.operator + " " + 
               prettyPrint(node.right) + ")";
    }
}