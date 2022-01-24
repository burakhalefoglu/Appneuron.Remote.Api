          env:
          - name: OperationClaimCrypto__Key
            valueFrom:
              secretKeyRef:
                name: app-secret
                key: opclaim-key
          - name: TokenOptions__AccessTokenExpiration
            value: "30"
          - name: TokenOptions__SecurityKey
            valueFrom:
              secretKeyRef:
                name: app-secret
                key: token-key
          - name: CorsPolicies
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: cors-policies
          - name: EmailConfiguration__SmtpServer
            value: appneuron.com
          - name: EmailConfiguration__SmtpPort
            value: "465"
          - name: EmailConfiguration__SmtpUsername
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: smtp-username
          - name: EmailConfiguration__SmtpPassword
            valueFrom:
              secretKeyRef:
                name: app-secret
                key: smtp-password
          - name: MongoDbSettings__Host
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: mongo-host
          - name: MongoDbSettings__Port
            value: "27017"
          - name: MongoDbSettings__UserName
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: mongo-user
          - name: MongoDbSettings__Password
            valueFrom:
              secretKeyRef:
                name: app-secret
                key: mongodb-root-password 
          - name: MessageBrokerOptions__HostName
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: kafka-host
          - name: MessageBrokerOptions__Port
            value: "9092"
          - name: SeriLogConfigurations__LogstashConfiguration__Host
            valueFrom:
              configMapKeyRef:
                name: app-config
                key: logstash-host
          - name: SeriLogConfigurations__LogstashConfiguration__Port
            value: "5000"