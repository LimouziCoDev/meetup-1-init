FROM alpine

# Copy the binary file and set it as entrypoint
ADD main /
ENTRYPOINT ["/main"]

# The service listens on port 8080 by default.
EXPOSE 8080