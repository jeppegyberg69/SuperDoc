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

		return { context, config };
	}
}
