# Generate sustained traffic to create visible metrics
for i in {1..100}; do
  curl http://localhost:5262/api/Product
  curl http://localhost:5262/api/Product/1
  curl http://localhost:5262/api/Product/2
  sleep 0.2
done

# Create some products
curl -X POST http://localhost:5262/api/Product \
  -H "Content-Type: application/json" \
  -d '{"name":"Dashboard Test Product","description":"Testing dashboard","price":99.99,"stockQuantity":10}'

# Test 404 errors
curl http://localhost:5262/api/Product/999
curl http://localhost:5262/api/Product/888