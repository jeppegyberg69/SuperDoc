const path = require('path')

module.exports = {
	module: {
		rules: [],
	},
	webpack: ({ context, config }) => {
		config.module.rules.push({
			test: /\.s[ac]ss$/i,
			use: [
				"style-loader",
				"css-loader",
				"sass-loader"
			]
		});


		// config.devServer.proxy = {
		// 	["https://localhost:44304/api/User"]: {
		// 		target: 'https://localhost:44304/',
		// 		changeOrigin: true,   // for vhosted sites, changes host header to match to target's host
		// 		headers: {
		// 			Origin: 'http://localhost:3000/',
		// 			Host: 'https://localhost:44304/'
		// 		},
		// 		secure: false,
		// 		autoRewrite: true,
		// 		protocolRewrite: 'http',
		// 	},
		// }

		return { context, config };
	}
}
